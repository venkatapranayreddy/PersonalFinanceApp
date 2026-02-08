using Microsoft.AspNetCore.Mvc;
using Pratice.Models;
using Pratice.Services.Interfaces;

namespace Pratice.Controllers;

public class IncomeController : BaseController
{
    private readonly IIncomeService _incomeService;
    private readonly ICategoryService _categoryService;

    public IncomeController(IIncomeService incomeService, ICategoryService categoryService)
    {
        _incomeService = incomeService;
        _categoryService = categoryService;
    }

    public IActionResult Index(int? month, int? year)
    {
        var now = DateTime.Today;
        var selectedMonth = month ?? now.Month;
        var selectedYear = year ?? now.Year;

        var start = new DateTime(selectedYear, selectedMonth, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var incomes = _incomeService.GetByDateRange(start, end, UserId);

        ViewBag.SelectedMonth = selectedMonth;
        ViewBag.SelectedYear = selectedYear;
        ViewBag.TotalIncome = incomes.Sum(i => i.Amount);

        return View(incomes);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = _categoryService.GetIncomeCategorySelectList(UserId);
        return View(new Income { Date = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Income income)
    {
        if (ModelState.IsValid)
        {
            _incomeService.Create(income, UserId);
            TempData["Success"] = "Income added successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetIncomeCategorySelectList(UserId, income.CategoryId);
        return View(income);
    }

    public IActionResult Edit(Guid id)
    {
        var income = _incomeService.GetById(id, UserId);
        if (income == null)
        {
            TempData["Error"] = "Income not found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetIncomeCategorySelectList(UserId, income.CategoryId);
        return View(income);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Income income)
    {
        if (ModelState.IsValid)
        {
            _incomeService.Update(income, UserId);
            TempData["Success"] = "Income updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetIncomeCategorySelectList(UserId, income.CategoryId);
        return View(income);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid id)
    {
        _incomeService.Delete(id, UserId);
        TempData["Success"] = "Income deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}
