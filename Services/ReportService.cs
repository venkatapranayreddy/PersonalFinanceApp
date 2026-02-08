using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Pratice.Data;
using Pratice.Services.Interfaces;
using Pratice.ViewModels;

namespace Pratice.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;
    private readonly IExpenseService _expenseService;
    private readonly IIncomeService _incomeService;
    private readonly IBudgetService _budgetService;

    public ReportService(ApplicationDbContext context, IExpenseService expenseService,
        IIncomeService incomeService, IBudgetService budgetService)
    {
        _context = context;
        _expenseService = expenseService;
        _incomeService = incomeService;
        _budgetService = budgetService;
    }

    public ReportsViewModel GetMonthlyReport(int month, int year, string userId)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var totalIncome = _incomeService.GetTotalForMonth(month, year, userId);
        var totalExpenses = _expenseService.GetTotalForMonth(month, year, userId);
        var categoryBreakdown = GetCategoryBreakdown(start, end, userId);
        var trends = GetSpendingTrends(6, userId);
        var budgets = _budgetService.GetForMonth(month, year, userId);

        var pieChartData = new
        {
            labels = categoryBreakdown.Select(c => c.CategoryName).ToArray(),
            values = categoryBreakdown.Select(c => (double)c.Amount).ToArray(),
            colors = categoryBreakdown.Select(c => c.Color).ToArray()
        };

        var trendChartData = new
        {
            labels = trends.Select(t => t.Month).ToArray(),
            income = trends.Select(t => (double)t.Income).ToArray(),
            expenses = trends.Select(t => (double)t.Expenses).ToArray()
        };

        var budgetChartData = new
        {
            labels = budgets.Select(b => b.Category?.Name ?? "Unknown").ToArray(),
            budget = budgets.Select(b => (double)b.Amount).ToArray(),
            spent = budgets.Select(b => (double)b.SpentAmount).ToArray()
        };

        return new ReportsViewModel
        {
            SelectedMonth = month,
            SelectedYear = year,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            CategoryBreakdowns = categoryBreakdown,
            CategoryPieChartJson = JsonSerializer.Serialize(pieChartData),
            SpendingTrendsJson = JsonSerializer.Serialize(trendChartData),
            BudgetPerformanceJson = JsonSerializer.Serialize(budgetChartData)
        };
    }

    public List<MonthlyTrend> GetSpendingTrends(int months, string userId)
    {
        var trends = new List<MonthlyTrend>();
        var today = DateTime.Today;

        for (int i = months - 1; i >= 0; i--)
        {
            var date = today.AddMonths(-i);
            var income = _incomeService.GetTotalForMonth(date.Month, date.Year, userId);
            var expenses = _expenseService.GetTotalForMonth(date.Month, date.Year, userId);

            trends.Add(new MonthlyTrend
            {
                Month = date.ToString("MMM yyyy"),
                Income = income,
                Expenses = expenses,
                Savings = income - expenses
            });
        }

        return trends;
    }

    public List<CategoryBreakdown> GetCategoryBreakdown(DateTime start, DateTime end, string userId)
    {
        var expenses = _context.Expenses.Include(e => e.Category)
            .Where(e => e.UserId == userId && e.Date >= start && e.Date <= end)
            .ToList();
        var totalExpenses = expenses.Sum(e => e.Amount);

        if (totalExpenses == 0)
            return new List<CategoryBreakdown>();

        return expenses
            .GroupBy(e => new { e.CategoryId, e.Category?.Name, e.Category?.Color })
            .Select(g => new CategoryBreakdown
            {
                CategoryName = g.Key.Name ?? "Unknown",
                Color = g.Key.Color ?? "#6c757d",
                Amount = g.Sum(e => e.Amount),
                Percentage = (double)(g.Sum(e => e.Amount) / totalExpenses) * 100
            })
            .OrderByDescending(c => c.Amount)
            .ToList();
    }
}
