namespace Pratice.ViewModels;

using Pratice.Models;

public class DashboardViewModel
{
    public decimal TotalIncomeThisMonth { get; set; }
    public decimal TotalExpensesThisMonth { get; set; }
    public decimal NetSavingsThisMonth => TotalIncomeThisMonth - TotalExpensesThisMonth;
    public decimal NetWorth { get; set; }
    public decimal TotalAssets { get; set; }
    public decimal TotalLiabilities { get; set; }

    public List<Expense> RecentExpenses { get; set; } = new();
    public List<Income> RecentIncomes { get; set; } = new();
    public List<Budget> BudgetAlerts { get; set; } = new();

    public string ExpenseByCategoryJson { get; set; } = "{}";
    public string MonthlyTrendJson { get; set; } = "[]";

    public int CurrentMonth { get; set; } = DateTime.Now.Month;
    public int CurrentYear { get; set; } = DateTime.Now.Year;
}
