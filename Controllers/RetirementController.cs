using Microsoft.AspNetCore.Mvc;
using Pratice.Models;
using Pratice.Models.Enums;
using Pratice.Services.Interfaces;
using Pratice.ViewModels;

namespace Pratice.Controllers;

public class RetirementController : BaseController
{
    private readonly IRetirementService _retirementService;

    public RetirementController(IRetirementService retirementService)
    {
        _retirementService = retirementService;
    }

    public IActionResult Onboarding()
    {
        var profile = _retirementService.GetByUserId(UserId);
        if (profile != null)
        {
            // Pre-fill with existing data for editing
            return View(new RetirementViewModel
            {
                CurrentAge = profile.CurrentAge,
                EmploymentStatus = profile.EmploymentStatus,
                RetirementAge = profile.RetirementAge,
                MonthlyIncome = profile.MonthlyIncome,
                MonthlyExpenses = profile.MonthlyExpenses,
                MonthlyInvestment = profile.MonthlyInvestment,
                ExpectedReturnRate = profile.ExpectedReturnRate,
                RetirementGoalAmount = profile.RetirementGoalAmount,
                StepUps = profile.StepUps
                    .OrderBy(s => s.AfterMonth)
                    .Select(s => new StepUpEntry { AfterMonth = s.AfterMonth, Amount = s.Amount })
                    .ToList(),
                HasProfile = true
            });
        }

        return View(new RetirementViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SaveProfile(RetirementViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Onboarding", model);

        if (model.RetirementAge <= model.CurrentAge)
        {
            ModelState.AddModelError("RetirementAge", "Retirement age must be greater than your current age.");
            return View("Onboarding", model);
        }

        // Validate: investment cannot exceed savings
        var maxInvestment = model.MonthlyIncome - model.MonthlyExpenses;
        if (model.MonthlyInvestment > maxInvestment)
        {
            ModelState.AddModelError("MonthlyInvestment",
                $"Monthly investment cannot exceed your monthly savings ({maxInvestment:C}).");
            return View("Onboarding", model);
        }

        // Validate step-up amounts against savings
        if (model.StepUps != null)
        {
            foreach (var stepUp in model.StepUps)
            {
                if (stepUp.Amount > maxInvestment)
                {
                    ModelState.AddModelError("",
                        $"Step-up amount {stepUp.Amount:C} at month {stepUp.AfterMonth} exceeds your monthly savings ({maxInvestment:C}).");
                    return View("Onboarding", model);
                }
            }
        }

        var existing = _retirementService.GetByUserId(UserId);

        if (existing != null)
        {
            existing.CurrentAge = model.CurrentAge;
            existing.EmploymentStatus = model.EmploymentStatus;
            existing.RetirementAge = model.RetirementAge;
            existing.MonthlyIncome = model.MonthlyIncome;
            existing.MonthlyExpenses = model.MonthlyExpenses;
            existing.MonthlyInvestment = model.MonthlyInvestment;
            existing.ExpectedReturnRate = model.ExpectedReturnRate;
            existing.RetirementGoalAmount = model.RetirementGoalAmount;
            _retirementService.Update(existing);

            // Save step-ups
            _retirementService.SaveStepUps(existing.Id,
                model.StepUps?.Where(s => s.AfterMonth > 0 && s.Amount > 0).ToList() ?? new());
        }
        else
        {
            var profile = new RetirementProfile
            {
                UserId = UserId,
                CurrentAge = model.CurrentAge,
                EmploymentStatus = model.EmploymentStatus,
                RetirementAge = model.RetirementAge,
                MonthlyIncome = model.MonthlyIncome,
                MonthlyExpenses = model.MonthlyExpenses,
                MonthlyInvestment = model.MonthlyInvestment,
                ExpectedReturnRate = model.ExpectedReturnRate,
                RetirementGoalAmount = model.RetirementGoalAmount
            };
            _retirementService.Create(profile);

            // Save step-ups
            _retirementService.SaveStepUps(profile.Id,
                model.StepUps?.Where(s => s.AfterMonth > 0 && s.Amount > 0).ToList() ?? new());
        }

        TempData["Success"] = existing != null ? "Retirement plan updated successfully!" : "Retirement plan created successfully!";
        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        var profile = _retirementService.GetByUserId(UserId);
        if (profile == null)
            return RedirectToAction("Onboarding");

        var stepUps = profile.StepUps
            .OrderBy(s => s.AfterMonth)
            .Select(s => new StepUpEntry { AfterMonth = s.AfterMonth, Amount = s.Amount })
            .ToList();

        var result = _retirementService.CalculateRetirementPlan(
            profile.CurrentAge, profile.RetirementAge,
            profile.MonthlyInvestment, profile.MonthlyExpenses,
            profile.ExpectedReturnRate, profile.RetirementGoalAmount,
            stepUps);

        // Populate input fields from saved profile
        result.CurrentAge = profile.CurrentAge;
        result.EmploymentStatus = profile.EmploymentStatus;
        result.RetirementAge = profile.RetirementAge;
        result.MonthlyIncome = profile.MonthlyIncome;
        result.MonthlyExpenses = profile.MonthlyExpenses;
        result.MonthlyInvestment = profile.MonthlyInvestment;
        result.ExpectedReturnRate = profile.ExpectedReturnRate;
        result.RetirementGoalAmount = profile.RetirementGoalAmount;
        result.MonthlySavings = profile.MonthlyIncome - profile.MonthlyExpenses;
        result.StepUps = stepUps;

        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdatePlan(RetirementViewModel model)
    {
        if (model.RetirementAge <= model.CurrentAge)
        {
            return Json(new { success = false, error = "Retirement age must be greater than current age." });
        }

        // Validate: investment cannot exceed savings
        var maxInvestment = model.MonthlyIncome - model.MonthlyExpenses;
        if (model.MonthlyInvestment > maxInvestment)
        {
            return Json(new { success = false, error = $"Monthly investment cannot exceed your monthly savings ({maxInvestment:C})." });
        }

        // Validate step-up amounts
        if (model.StepUps != null)
        {
            foreach (var stepUp in model.StepUps)
            {
                if (stepUp.Amount > maxInvestment)
                {
                    return Json(new { success = false, error = $"Step-up amount {stepUp.Amount:C} exceeds your monthly savings ({maxInvestment:C})." });
                }
            }
        }

        // Update the saved profile
        var profile = _retirementService.GetByUserId(UserId);
        if (profile != null)
        {
            profile.MonthlyInvestment = model.MonthlyInvestment;
            profile.RetirementAge = model.RetirementAge;
            profile.ExpectedReturnRate = model.ExpectedReturnRate;
            profile.CurrentAge = model.CurrentAge;
            profile.MonthlyExpenses = model.MonthlyExpenses;
            profile.MonthlyIncome = model.MonthlyIncome;
            profile.RetirementGoalAmount = model.RetirementGoalAmount;
            _retirementService.Update(profile);

            // Save step-ups
            _retirementService.SaveStepUps(profile.Id,
                model.StepUps?.Where(s => s.AfterMonth > 0 && s.Amount > 0).ToList() ?? new());
        }

        var stepUps = model.StepUps?.Where(s => s.AfterMonth > 0 && s.Amount > 0).ToList() ?? new();

        var result = _retirementService.CalculateRetirementPlan(
            model.CurrentAge, model.RetirementAge,
            model.MonthlyInvestment, model.MonthlyExpenses,
            model.ExpectedReturnRate, model.RetirementGoalAmount,
            stepUps);

        return Json(new
        {
            success = true,
            yearsToRetirement = result.YearsToRetirement,
            totalInvested = result.TotalInvested,
            expectedCorpus = result.ExpectedCorpus,
            estimatedReturns = result.EstimatedReturns,
            recommendedMonthlyInvestment = result.RecommendedMonthlyInvestment,
            retirementCorpusNeeded = result.RetirementCorpusNeeded,
            yearlyBreakdownJson = result.YearlyBreakdownJson,
            yearlyBreakdownTable = result.YearlyBreakdownTable,
            monthsToReachGoal = result.MonthsToReachGoal,
            goalAchievable = result.GoalAchievable
        });
    }
}
