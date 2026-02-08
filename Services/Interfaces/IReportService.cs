using Pratice.ViewModels;

namespace Pratice.Services.Interfaces;

public interface IReportService
{
    ReportsViewModel GetMonthlyReport(int month, int year, string userId);
    List<MonthlyTrend> GetSpendingTrends(int months, string userId);
    List<CategoryBreakdown> GetCategoryBreakdown(DateTime start, DateTime end, string userId);
}
