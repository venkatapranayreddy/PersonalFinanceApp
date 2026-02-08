using Microsoft.AspNetCore.Mvc;
using Pratice.Services.Interfaces;

namespace Pratice.Controllers;

public class ReportsController : BaseController
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public IActionResult Index(int? month, int? year)
    {
        var now = DateTime.Today;
        var selectedMonth = month ?? now.Month;
        var selectedYear = year ?? now.Year;

        var viewModel = _reportService.GetMonthlyReport(selectedMonth, selectedYear, UserId);
        return View(viewModel);
    }
}
