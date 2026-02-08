namespace Pratice.ViewModels;

public class ReportsViewModel
{
    public int SelectedMonth { get; set; } = DateTime.Now.Month;
    public int SelectedYear { get; set; } = DateTime.Now.Year;

    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetSavings => TotalIncome - TotalExpenses;
    public double SavingsRate => TotalIncome > 0 ? (double)(NetSavings / TotalIncome) * 100 : 0;

    public string SpendingTrendsJson { get; set; } = "[]";
    public string IncomeVsExpenseJson { get; set; } = "[]";
    public string CategoryPieChartJson { get; set; } = "{}";
    public string BudgetPerformanceJson { get; set; } = "[]";

    public List<CategoryBreakdown> CategoryBreakdowns { get; set; } = new();
}

public class CategoryBreakdown
{
    public string CategoryName { get; set; } = string.Empty;
    public string Color { get; set; } = "#6c757d";
    public decimal Amount { get; set; }
    public double Percentage { get; set; }
}

public class MonthlyTrend
{
    public string Month { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
    public decimal Savings { get; set; }
}
