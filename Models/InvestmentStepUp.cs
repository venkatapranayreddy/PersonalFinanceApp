using System.ComponentModel.DataAnnotations;

namespace Pratice.Models;

public class InvestmentStepUp
{
    public Guid Id { get; set; }

    public Guid RetirementProfileId { get; set; }
    public RetirementProfile RetirementProfile { get; set; } = null!;

    [Required(ErrorMessage = "Month is required")]
    [Range(1, 600, ErrorMessage = "Month must be between 1 and 600")]
    [Display(Name = "After Month")]
    public int AfterMonth { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [Display(Name = "New Monthly Amount")]
    public decimal Amount { get; set; }
}
