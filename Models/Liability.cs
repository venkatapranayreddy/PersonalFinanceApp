using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Pratice.Models;

public class Liability
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [BindNever]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Range(0, 100, ErrorMessage = "Interest rate must be between 0 and 100")]
    public decimal? InterestRate { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MinimumPayment { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
