namespace Pratice.Models;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "bi-tag";
    public string Color { get; set; } = "#6c757d";
    public bool IsDefault { get; set; } = false;
    public bool IsExpenseCategory { get; set; } = true;
    public bool IsIncomeCategory { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Nullable: null = shared default category, set = user-custom category
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
