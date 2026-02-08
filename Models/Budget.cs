using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Pratice.Models;

public class Budget
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [BindNever]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Budget amount must be greater than 0")]
    public decimal Amount { get; set; }

    public decimal SpentAmount { get; set; } = 0;

    [Required]
    [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
    public int Month { get; set; }

    [Required]
    [Range(2000, 2100, ErrorMessage = "Please enter a valid year")]
    public int Year { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public decimal RemainingAmount => Amount - SpentAmount;
    public double PercentageUsed => Amount > 0 ? (double)(SpentAmount / Amount) * 100 : 0;
    public bool IsOverBudget => SpentAmount > Amount;
    public bool IsNearLimit => PercentageUsed >= 80 && !IsOverBudget;
}
