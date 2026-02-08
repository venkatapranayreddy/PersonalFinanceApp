using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pratice.Models.Enums;

namespace Pratice.Models;

public class Asset
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [BindNever]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Value must be 0 or greater")]
    public decimal Value { get; set; }

    [Required]
    public AssetType Type { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [DataType(DataType.Date)]
    public DateTime AsOfDate { get; set; } = DateTime.Today;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
