using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Pratice.Services.Interfaces;
using Pratice.ViewModels;

namespace Pratice.Controllers;

public class HomeController : BaseController
{
    private readonly IExpenseService _expenseService;
    private readonly IIncomeService _incomeService;
    private readonly IBudgetService _budgetService;
    private readonly INetWorthService _netWorthService;
    private readonly ICategoryService _categoryService;
    private readonly IRetirementService _retirementService;

    public HomeController(
        IExpenseService expenseService,
        IIncomeService incomeService,
        IBudgetService budgetService,
        INetWorthService netWorthService,
        ICategoryService categoryService,
        IRetirementService retirementService)
    {
        _expenseService = expenseService;
        _incomeService = incomeService;
        _budgetService = budgetService;
        _netWorthService = netWorthService;
        _categoryService = categoryService;
        _retirementService = retirementService;
    }

    public IActionResult Index()
    {
        // Redirect to onboarding if user hasn't set up retirement profile
        if (!_retirementService.HasProfile(UserId))
            return RedirectToAction("Onboarding", "Retirement");

        var now = DateTime.Today;
        var month = now.Month;
        var year = now.Year;

        var totalIncome = _incomeService.GetTotalForMonth(month, year, UserId);
        var totalExpenses = _expenseService.GetTotalForMonth(month, year, UserId);
        var budgetAlerts = _budgetService.GetBudgetsWithAlerts(month, year, UserId);
        var recentExpenses = _expenseService.GetRecent(5, UserId);
        var recentIncomes = _incomeService.GetRecent(5, UserId);

        // Category breakdown for pie chart
        var categoryTotals = _expenseService.GetTotalsByCategory(month, year, UserId);
        var categories = _categoryService.GetExpenseCategories(UserId).ToDictionary(c => c.Id, c => c);

        var pieData = categoryTotals.Select(ct =>
        {
            var cat = categories.TryGetValue(ct.Key, out var c) ? c : null;
            return new
            {
                name = cat?.Name ?? "Unknown",
                color = cat?.Color ?? "#6c757d",
                value = (double)ct.Value
            };
        }).ToList();

        var expenseByCategoryJson = JsonSerializer.Serialize(new
        {
            labels = pieData.Select(p => p.name).ToArray(),
            values = pieData.Select(p => p.value).ToArray(),
            colors = pieData.Select(p => p.color).ToArray()
        });

        // Monthly trend for line chart (last 6 months)
        var trendData = new List<object>();
        for (int i = 5; i >= 0; i--)
        {
            var trendMonth = now.AddMonths(-i);
            trendData.Add(new
            {
                month = trendMonth.ToString("MMM"),
                income = (double)_incomeService.GetTotalForMonth(trendMonth.Month, trendMonth.Year, UserId),
                expenses = (double)_expenseService.GetTotalForMonth(trendMonth.Month, trendMonth.Year, UserId)
            });
        }

        var monthlyTrendJson = JsonSerializer.Serialize(new
        {
            labels = trendData.Select(t => ((dynamic)t).month).ToArray(),
            income = trendData.Select(t => ((dynamic)t).income).ToArray(),
            expenses = trendData.Select(t => ((dynamic)t).expenses).ToArray()
        });

        var viewModel = new DashboardViewModel
        {
            TotalIncomeThisMonth = totalIncome,
            TotalExpensesThisMonth = totalExpenses,
            NetWorth = _netWorthService.GetNetWorth(UserId),
            TotalAssets = _netWorthService.GetTotalAssets(UserId),
            TotalLiabilities = _netWorthService.GetTotalLiabilities(UserId),
            RecentExpenses = recentExpenses,
            RecentIncomes = recentIncomes,
            BudgetAlerts = budgetAlerts,
            ExpenseByCategoryJson = expenseByCategoryJson,
            MonthlyTrendJson = monthlyTrendJson,
            CurrentMonth = month,
            CurrentYear = year
        };

        return View(viewModel);
    }

    public IActionResult Error()
    {
        return View();
    }
}
