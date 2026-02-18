using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pratice.Models.Enums;

namespace Pratice.Models;

public class RetirementProfile
{
    public Guid Id { get; set; }

    [BindNever]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

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

    public List<InvestmentStepUp> StepUps { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
