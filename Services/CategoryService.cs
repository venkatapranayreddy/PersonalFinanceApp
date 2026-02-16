using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pratice.Data;
using Pratice.Models;
using Pratice.Services.Interfaces;

namespace Pratice.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Category> GetExpenseCategories(string userId)
    {
        return _context.Categories
            .Where(c => c.IsExpenseCategory && (c.UserId == null || c.UserId == userId))
            .OrderBy(c => c.Name)
            .ToList();
    }

    public List<Category> GetIncomeCategories(string userId)
    {
        return _context.Categories
            .Where(c => c.IsIncomeCategory && (c.UserId == null || c.UserId == userId))
            .OrderBy(c => c.Name)
            .ToList();
    }

    public List<Category> GetAll(string userId)
    {
        return _context.Categories
            .Where(c => c.UserId == null || c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToList();
    }

    public Category? GetById(Guid id, string userId)
    {
        return _context.Categories
            .FirstOrDefault(c => c.Id == id && (c.UserId == null || c.UserId == userId));
    }

    public SelectList GetExpenseCategorySelectList(string userId, Guid? selectedId = null)
    {
        return new SelectList(GetExpenseCategories(userId), "Id", "Name", selectedId);
    }

    public SelectList GetIncomeCategorySelectList(string userId, Guid? selectedId = null)
    {
        return new SelectList(GetIncomeCategories(userId), "Id", "Name", selectedId);
    }

    public Category CreateExpenseCategory(string name, string userId)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Icon = "bi-tag",
            Color = "#6c757d",
            IsDefault = false,
            IsExpenseCategory = true,
            IsIncomeCategory = false,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.SaveChanges();
        return category;
    }
}
