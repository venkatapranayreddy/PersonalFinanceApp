using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pratice.Models.Enums;

namespace Pratice.Models;

public class Expense
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [BindNever]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [StringLength(500)]
    public string? Notes { get; set; }

    public string? PaymentMethod { get; set; }

    public bool IsRecurring { get; set; } = false;

    public FrequencyType? RecurringFrequency { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
