using Microsoft.AspNetCore.Mvc;
using Pratice.Models;
using Pratice.Services.Interfaces;

namespace Pratice.Controllers;

public class ExpenseController : BaseController
{
    private readonly IExpenseService _expenseService;
    private readonly ICategoryService _categoryService;

    public ExpenseController(IExpenseService expenseService, ICategoryService categoryService)
    {
        _expenseService = expenseService;
        _categoryService = categoryService;
    }

    public IActionResult Index(int? month, int? year)
    {
        var now = DateTime.Today;
        var selectedMonth = month ?? now.Month;
        var selectedYear = year ?? now.Year;

        var start = new DateTime(selectedYear, selectedMonth, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var expenses = _expenseService.GetByDateRange(start, end, UserId);

        ViewBag.SelectedMonth = selectedMonth;
        ViewBag.SelectedYear = selectedYear;
        ViewBag.TotalExpenses = expenses.Sum(e => e.Amount);

        return View(expenses);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = _categoryService.GetExpenseCategorySelectList(UserId);
        ViewBag.PaymentMethods = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            new[] { "Cash", "Credit Card", "Debit Card", "Bank Transfer", "Other" });
        return View(new Expense { Date = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Expense expense)
    {
        if (ModelState.IsValid)
        {
            _expenseService.Create(expense, UserId);
            TempData["Success"] = "Expense added successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetExpenseCategorySelectList(UserId, expense.CategoryId);
        ViewBag.PaymentMethods = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            new[] { "Cash", "Credit Card", "Debit Card", "Bank Transfer", "Other" }, expense.PaymentMethod);
        return View(expense);
    }

    public IActionResult Edit(Guid id)
    {
        var expense = _expenseService.GetById(id, UserId);
        if (expense == null)
        {
            TempData["Error"] = "Expense not found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetExpenseCategorySelectList(UserId, expense.CategoryId);
        ViewBag.PaymentMethods = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            new[] { "Cash", "Credit Card", "Debit Card", "Bank Transfer", "Other" }, expense.PaymentMethod);
        return View(expense);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Expense expense)
    {
        if (ModelState.IsValid)
        {
            _expenseService.Update(expense, UserId);
            TempData["Success"] = "Expense updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetExpenseCategorySelectList(UserId, expense.CategoryId);
        ViewBag.PaymentMethods = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            new[] { "Cash", "Credit Card", "Debit Card", "Bank Transfer", "Other" }, expense.PaymentMethod);
        return View(expense);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid id)
    {
        _expenseService.Delete(id, UserId);
        TempData["Success"] = "Expense deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}
