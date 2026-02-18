using System.ComponentModel.DataAnnotations;
using Pratice.Models.Enums;

namespace Pratice.ViewModels;

public class RetirementViewModel
{
    // Input fields (onboarding + adjustment)
    [Required(ErrorMessage = "Age is required")]
    [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
    [Display(Name = "Current Age")]
    public int CurrentAge { get; set; }

    [Required(ErrorMessage = "Employment status is required")]
    [Display(Name = "Employment Status")]
    public EmploymentStatus EmploymentStatus { get; set; }

    [Required(ErrorMessage = "Retirement age is required")]
    [Range(30, 100, ErrorMessage = "Retirement age must be between 30 and 100")]
    [Display(Name = "Retirement Age")]
    public int RetirementAge { get; set; }

    [Required(ErrorMessage = "Monthly income is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Monthly income must be greater than 0")]
    [Display(Name = "Monthly Income")]
    public decimal MonthlyIncome { get; set; }

    [Required(ErrorMessage = "Monthly expenses are required")]
    [Range(0, double.MaxValue, ErrorMessage = "Monthly expenses cannot be negative")]
    [Display(Name = "Monthly Expenses")]
    public decimal MonthlyExpenses { get; set; }

    [Required(ErrorMessage = "Monthly investment is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Monthly investment must be greater than 0")]
    [Display(Name = "Monthly Investment")]
    public decimal MonthlyInvestment { get; set; }

    [Range(1, 30, ErrorMessage = "Expected return rate must be between 1% and 30%")]
    [Display(Name = "Expected Return Rate (%)")]
    public decimal ExpectedReturnRate { get; set; } = 12;

    [Required(ErrorMessage = "Retirement goal amount is required")]
    [Range(1, double.MaxValue, ErrorMessage = "Retirement goal must be greater than 0")]
    [Display(Name = "Retirement Goal Amount")]
    public decimal RetirementGoalAmount { get; set; }

    // Step-ups
    public List<StepUpEntry> StepUps { get; set; } = new();

    // Computed results
    public bool HasProfile { get; set; }
    public int YearsToRetirement { get; set; }
    public decimal TotalInvested { get; set; }
    public decimal ExpectedCorpus { get; set; }
    public decimal EstimatedReturns { get; set; }
    public decimal RecommendedMonthlyInvestment { get; set; }
    public decimal RetirementCorpusNeeded { get; set; }
    public decimal MonthlySavings { get; set; }
    public int? MonthsToReachGoal { get; set; }
    public bool GoalAchievable { get; set; }

    // Chart and table data
    public string YearlyBreakdownJson { get; set; } = "{}";
    public List<YearlyBreakdown> YearlyBreakdownTable { get; set; } = new();
}

public class YearlyBreakdown
{
    public int Year { get; set; }
    public int Age { get; set; }
    public decimal AmountInvested { get; set; }
    public decimal ExpectedValue { get; set; }
    public decimal ReturnsEarned { get; set; }
    public decimal MonthlyInvestmentAmount { get; set; }
}

public class StepUpEntry
{
    public int AfterMonth { get; set; }
    public decimal Amount { get; set; }
}
