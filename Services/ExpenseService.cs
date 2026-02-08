using Microsoft.EntityFrameworkCore;
using Pratice.Data;
using Pratice.Models;
using Pratice.Services.Interfaces;

namespace Pratice.Services;

public class ExpenseService : IExpenseService
{
    private readonly ApplicationDbContext _context;

    public ExpenseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Expense> GetAll(string userId)
    {
        return _context.Expenses.Include(e => e.Category)
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Date).ToList();
    }

    public Expense? GetById(Guid id, string userId)
    {
        return _context.Expenses.Include(e => e.Category)
            .FirstOrDefault(e => e.Id == id && e.UserId == userId);
    }

    public void Create(Expense expense, string userId)
    {
        expense.Id = Guid.NewGuid();
        expense.UserId = userId;
        expense.CreatedAt = DateTime.UtcNow;
        _context.Expenses.Add(expense);
        _context.SaveChanges();
    }

    public void Update(Expense expense, string userId)
    {
        var existing = _context.Expenses.FirstOrDefault(e => e.Id == expense.Id && e.UserId == userId);
        if (existing == null) return;

        existing.Amount = expense.Amount;
        existing.Description = expense.Description;
        existing.CategoryId = expense.CategoryId;
        existing.Date = expense.Date;
        existing.Notes = expense.Notes;
        existing.PaymentMethod = expense.PaymentMethod;
        existing.IsRecurring = expense.IsRecurring;
        existing.RecurringFrequency = expense.RecurringFrequency;
        existing.UpdatedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }

    public void Delete(Guid id, string userId)
    {
        var expense = _context.Expenses.FirstOrDefault(e => e.Id == id && e.UserId == userId);
        if (expense == null) return;
        _context.Expenses.Remove(expense);
        _context.SaveChanges();
    }

    public List<Expense> GetByDateRange(DateTime start, DateTime end, string userId)
    {
        return _context.Expenses.Include(e => e.Category)
            .Where(e => e.UserId == userId && e.Date >= start && e.Date <= end)
            .OrderByDescending(e => e.Date).ToList();
    }

    public List<Expense> GetByCategory(Guid categoryId, string userId)
    {
        return _context.Expenses.Include(e => e.Category)
            .Where(e => e.UserId == userId && e.CategoryId == categoryId)
            .OrderByDescending(e => e.Date).ToList();
    }

    public List<Expense> GetCurrentMonthExpenses(string userId)
    {
        var now = DateTime.Today;
        var start = new DateTime(now.Year, now.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        return GetByDateRange(start, end, userId);
    }

    public decimal GetTotalForMonth(int month, int year, string userId)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        return _context.Expenses
            .Where(e => e.UserId == userId && e.Date >= start && e.Date <= end)
            .Sum(e => e.Amount);
    }

    public Dictionary<Guid, decimal> GetTotalsByCategory(int month, int year, string userId)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        return _context.Expenses
            .Where(e => e.UserId == userId && e.Date >= start && e.Date <= end)
            .GroupBy(e => e.CategoryId)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
    }

    public List<Expense> GetRecent(int count, string userId)
    {
        return _context.Expenses.Include(e => e.Category)
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Date)
            .ThenByDescending(e => e.CreatedAt)
            .Take(count).ToList();
    }
}
