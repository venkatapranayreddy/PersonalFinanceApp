using Microsoft.AspNetCore.Mvc;
using Pratice.Models;
using Pratice.Services.Interfaces;

namespace Pratice.Controllers;

public class BudgetController : BaseController
{
    private readonly IBudgetService _budgetService;
    private readonly ICategoryService _categoryService;

    public BudgetController(IBudgetService budgetService, ICategoryService categoryService)
    {
        _budgetService = budgetService;
        _categoryService = categoryService;
    }

    public IActionResult Index(int? month, int? year)
    {
        var now = DateTime.Today;
        var selectedMonth = month ?? now.Month;
        var selectedYear = year ?? now.Year;

        var budgets = _budgetService.GetForMonth(selectedMonth, selectedYear, UserId);

        ViewBag.SelectedMonth = selectedMonth;
        ViewBag.SelectedYear = selectedYear;
        ViewBag.TotalBudget = budgets.Sum(b => b.Amount);
        ViewBag.TotalSpent = budgets.Sum(b => b.SpentAmount);

        return View(budgets);
    }

    public IActionResult Create()
    {
        var now = DateTime.Today;
        ViewBag.Categories = _categoryService.GetExpenseCategorySelectList(UserId);
        return View(new Budget { Month = now.Month, Year = now.Year });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Budget budget)
    {
        var existing = _budgetService.GetByCategoryAndMonth(budget.CategoryId, budget.Month, budget.Year, UserId);
        if (existing != null)
        {
            ModelState.AddModelError("CategoryId", "A budget for this category already exists for the selected month.");
        }

        if (ModelState.IsValid)
        {
            _budgetService.Create(budget, UserId);
            TempData["Success"] = "Budget created successfully!";
            return RedirectToAction(nameof(Index), new { month = budget.Month, year = budget.Year });
        }

        ViewBag.Categories = _categoryService.GetExpenseCategorySelectList(UserId, budget.CategoryId);
        return View(budget);
    }

    public IActionResult Edit(Guid id)
    {
        var budget = _budgetService.GetById(id, UserId);
        if (budget == null)
        {
            TempData["Error"] = "Budget not found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetExpenseCategorySelectList(UserId, budget.CategoryId);
        return View(budget);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Budget budget)
    {
        if (ModelState.IsValid)
        {
            _budgetService.Update(budget, UserId);
            TempData["Success"] = "Budget updated successfully!";
            return RedirectToAction(nameof(Index), new { month = budget.Month, year = budget.Year });
        }

        ViewBag.Categories = _categoryService.GetExpenseCategorySelectList(UserId, budget.CategoryId);
        return View(budget);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid id)
    {
        var budget = _budgetService.GetById(id, UserId);
        _budgetService.Delete(id, UserId);
        TempData["Success"] = "Budget deleted successfully!";
        return RedirectToAction(nameof(Index), new { month = budget?.Month, year = budget?.Year });
    }
}
