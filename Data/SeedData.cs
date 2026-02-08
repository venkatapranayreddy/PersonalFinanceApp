using Microsoft.EntityFrameworkCore;
using Pratice.Models;

namespace Pratice.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var expenseCategories = new List<Category>
        {
            new() { Name = "Food & Dining", Icon = "bi-cup-hot", Color = "#ff6b6b", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Transportation", Icon = "bi-car-front", Color = "#4ecdc4", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Housing", Icon = "bi-house", Color = "#45b7d1", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Utilities", Icon = "bi-lightning", Color = "#96ceb4", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Entertainment", Icon = "bi-film", Color = "#dda0dd", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Shopping", Icon = "bi-bag", Color = "#ffeaa7", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Healthcare", Icon = "bi-heart-pulse", Color = "#fd79a8", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Education", Icon = "bi-book", Color = "#a29bfe", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Personal Care", Icon = "bi-person", Color = "#74b9ff", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false },
            new() { Name = "Other Expense", Icon = "bi-three-dots", Color = "#636e72", IsDefault = true, IsExpenseCategory = true, IsIncomeCategory = false }
        };

        var incomeCategories = new List<Category>
        {
            new() { Name = "Salary", Icon = "bi-briefcase", Color = "#00b894", IsDefault = true, IsExpenseCategory = false, IsIncomeCategory = true },
            new() { Name = "Freelance", Icon = "bi-laptop", Color = "#00cec9", IsDefault = true, IsExpenseCategory = false, IsIncomeCategory = true },
            new() { Name = "Investments", Icon = "bi-graph-up", Color = "#6c5ce7", IsDefault = true, IsExpenseCategory = false, IsIncomeCategory = true },
            new() { Name = "Dividends", Icon = "bi-cash-stack", Color = "#e17055", IsDefault = true, IsExpenseCategory = false, IsIncomeCategory = true },
            new() { Name = "Rental Income", Icon = "bi-building", Color = "#fdcb6e", IsDefault = true, IsExpenseCategory = false, IsIncomeCategory = true },
            new() { Name = "Side Business", Icon = "bi-shop", Color = "#e84393", IsDefault = true, IsExpenseCategory = false, IsIncomeCategory = true },
            new() { Name = "Gifts", Icon = "bi-gift", Color = "#ff7675", IsDefault = true, IsExpenseCategory = false, IsIncomeCategory = true },
            new() { Name = "Other Income", Icon = "bi-three-dots", Color = "#636e72", IsDefault = true, IsExpenseCategory = false, IsIncomeCategory = true }
        };

        context.Categories.AddRange(expenseCategories);
        context.Categories.AddRange(incomeCategories);
        await context.SaveChangesAsync();
    }
}
