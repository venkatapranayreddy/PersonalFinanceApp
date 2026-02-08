using Microsoft.EntityFrameworkCore;
using Pratice.Data;
using Pratice.Models;
using Pratice.Services.Interfaces;

namespace Pratice.Services;

public class BudgetService : IBudgetService
{
    private readonly ApplicationDbContext _context;
    private readonly IExpenseService _expenseService;

    public BudgetService(ApplicationDbContext context, IExpenseService expenseService)
    {
        _context = context;
        _expenseService = expenseService;
    }

    public List<Budget> GetAll(string userId)
    {
        return _context.Budgets.Include(b => b.Category)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Year).ThenByDescending(b => b.Month).ToList();
    }

    public List<Budget> GetForMonth(int month, int year, string userId)
    {
        var budgets = _context.Budgets.Include(b => b.Category)
            .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
            .ToList();
        RecalculateSpentAmountsInternal(budgets, month, year, userId);
        return budgets;
    }

    public Budget? GetById(Guid id, string userId)
    {
        var budget = _context.Budgets.Include(b => b.Category)
            .FirstOrDefault(b => b.Id == id && b.UserId == userId);
        if (budget != null)
        {
            RecalculateSpentAmountsInternal(new List<Budget> { budget }, budget.Month, budget.Year, userId);
        }
        return budget;
    }

    public Budget? GetByCategoryAndMonth(Guid categoryId, int month, int year, string userId)
    {
        return _context.Budgets
            .FirstOrDefault(b => b.UserId == userId && b.CategoryId == categoryId && b.Month == month && b.Year == year);
    }

    public void Create(Budget budget, string userId)
    {
        budget.Id = Guid.NewGuid();
        budget.UserId = userId;
        budget.CreatedAt = DateTime.UtcNow;
        _context.Budgets.Add(budget);
        _context.SaveChanges();
    }

    public void Update(Budget budget, string userId)
    {
        var existing = _context.Budgets.FirstOrDefault(b => b.Id == budget.Id && b.UserId == userId);
        if (existing == null) return;

        existing.Amount = budget.Amount;
        existing.UpdatedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }

    public void Delete(Guid id, string userId)
    {
        var budget = _context.Budgets.FirstOrDefault(b => b.Id == id && b.UserId == userId);
        if (budget == null) return;
        _context.Budgets.Remove(budget);
        _context.SaveChanges();
    }

    public void RecalculateSpentAmounts(int month, int year, string userId)
    {
        var budgets = _context.Budgets
            .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
            .ToList();
        RecalculateSpentAmountsInternal(budgets, month, year, userId);
        _context.SaveChanges();
    }

    private void RecalculateSpentAmountsInternal(List<Budget> budgets, int month, int year, string userId)
    {
        var categoryTotals = _expenseService.GetTotalsByCategory(month, year, userId);
        foreach (var budget in budgets)
        {
            budget.SpentAmount = categoryTotals.TryGetValue(budget.CategoryId, out var spent) ? spent : 0;
        }
    }

    public List<Budget> GetBudgetsWithAlerts(int month, int year, string userId)
    {
        var budgets = GetForMonth(month, year, userId);
        return budgets.Where(b => b.IsNearLimit || b.IsOverBudget).ToList();
    }
}
