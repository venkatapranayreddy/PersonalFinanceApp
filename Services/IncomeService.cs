using Microsoft.EntityFrameworkCore;
using Pratice.Data;
using Pratice.Models;
using Pratice.Services.Interfaces;

namespace Pratice.Services;

public class IncomeService : IIncomeService
{
    private readonly ApplicationDbContext _context;

    public IncomeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Income> GetAll(string userId)
    {
        return _context.Incomes.Include(i => i.Category)
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.Date).ToList();
    }

    public Income? GetById(Guid id, string userId)
    {
        return _context.Incomes.Include(i => i.Category)
            .FirstOrDefault(i => i.Id == id && i.UserId == userId);
    }

    public void Create(Income income, string userId)
    {
        income.Id = Guid.NewGuid();
        income.UserId = userId;
        income.CreatedAt = DateTime.UtcNow;
        _context.Incomes.Add(income);
        _context.SaveChanges();
    }

    public void Update(Income income, string userId)
    {
        var existing = _context.Incomes.FirstOrDefault(i => i.Id == income.Id && i.UserId == userId);
        if (existing == null) return;

        existing.Amount = income.Amount;
        existing.Source = income.Source;
        existing.CategoryId = income.CategoryId;
        existing.Date = income.Date;
        existing.Notes = income.Notes;
        existing.IsRecurring = income.IsRecurring;
        existing.RecurringFrequency = income.RecurringFrequency;
        existing.UpdatedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }

    public void Delete(Guid id, string userId)
    {
        var income = _context.Incomes.FirstOrDefault(i => i.Id == id && i.UserId == userId);
        if (income == null) return;
        _context.Incomes.Remove(income);
        _context.SaveChanges();
    }

    public List<Income> GetByDateRange(DateTime start, DateTime end, string userId)
    {
        return _context.Incomes.Include(i => i.Category)
            .Where(i => i.UserId == userId && i.Date >= start && i.Date <= end)
            .OrderByDescending(i => i.Date).ToList();
    }

    public List<Income> GetCurrentMonthIncomes(string userId)
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
        return _context.Incomes
            .Where(i => i.UserId == userId && i.Date >= start && i.Date <= end)
            .Sum(i => i.Amount);
    }

    public List<Income> GetRecent(int count, string userId)
    {
        return _context.Incomes.Include(i => i.Category)
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.Date)
            .ThenByDescending(i => i.CreatedAt)
            .Take(count).ToList();
    }
}
