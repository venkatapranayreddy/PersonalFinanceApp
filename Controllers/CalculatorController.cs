using Microsoft.AspNetCore.Mvc;
using Pratice.Services.Interfaces;
using Pratice.ViewModels;

namespace Pratice.Controllers;

public class CalculatorController : BaseController
{
    private readonly IInvestmentCalculatorService _calculatorService;

    public CalculatorController(IInvestmentCalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    public IActionResult Index()
    {
        return View(new InvestmentCalculatorViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CalculateSip(InvestmentCalculatorViewModel model)
    {
        if (model.MonthlyInvestment == null || model.MonthlyInvestment <= 0)
        {
            ModelState.AddModelError(nameof(model.MonthlyInvestment), "Monthly investment amount is required and must be greater than 0.");
        }

        if (model.TimePeriodYears < 1 || model.TimePeriodYears > 50)
        {
            ModelState.AddModelError(nameof(model.TimePeriodYears), "Time period must be between 1 and 50 years.");
        }

        if (model.ExpectedReturnRate < 0.1m || model.ExpectedReturnRate > 100)
        {
            ModelState.AddModelError(nameof(model.ExpectedReturnRate), "Return rate must be between 0.1% and 100%.");
        }

        if (!ModelState.IsValid)
        {
            model.CalculatorType = "SIP";
            return View("Index", model);
        }

        var result = _calculatorService.CalculateSip(model.MonthlyInvestment!.Value, model.TimePeriodYears, model.ExpectedReturnRate);
        return View("Index", result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CalculateLumpSum(InvestmentCalculatorViewModel model)
    {
        if (model.LumpSumAmount == null || model.LumpSumAmount <= 0)
        {
            ModelState.AddModelError(nameof(model.LumpSumAmount), "Investment amount is required and must be greater than 0.");
        }

        if (model.TimePeriodYears < 1 || model.TimePeriodYears > 50)
        {
            ModelState.AddModelError(nameof(model.TimePeriodYears), "Time period must be between 1 and 50 years.");
        }

        if (model.ExpectedReturnRate < 0.1m || model.ExpectedReturnRate > 100)
        {
            ModelState.AddModelError(nameof(model.ExpectedReturnRate), "Return rate must be between 0.1% and 100%.");
        }

        if (!ModelState.IsValid)
        {
            model.CalculatorType = "LumpSum";
            return View("Index", model);
        }

        var result = _calculatorService.CalculateLumpSum(model.LumpSumAmount!.Value, model.TimePeriodYears, model.ExpectedReturnRate);
        return View("Index", result);
    }
}
