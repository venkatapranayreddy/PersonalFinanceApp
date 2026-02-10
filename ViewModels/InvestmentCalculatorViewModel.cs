using System.ComponentModel.DataAnnotations;

namespace Pratice.ViewModels;

public class InvestmentCalculatorViewModel
{
    public string CalculatorType { get; set; } = "SIP";

    [Display(Name = "Monthly Investment Amount")]
    [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal? MonthlyInvestment { get; set; }

    [Display(Name = "One-Time Investment Amount")]
    [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal? LumpSumAmount { get; set; }

    [Display(Name = "Time Period (Years)")]
    [Range(1, 50, ErrorMessage = "Time period must be between 1 and 50 years")]
    public int TimePeriodYears { get; set; }

    [Display(Name = "Expected Return Rate (% p.a.)")]
    [Range(0.1, 100, ErrorMessage = "Rate must be between 0.1% and 100%")]
    public decimal ExpectedReturnRate { get; set; }

    public decimal TotalInvested { get; set; }
    public decimal EstimatedReturns { get; set; }
    public decimal TotalValue { get; set; }
    public bool HasResult { get; set; }

    public string YearlyBreakdownJson { get; set; } = "{}";
}
