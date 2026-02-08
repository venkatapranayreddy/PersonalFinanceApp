using Pratice.Models;

namespace Pratice.Services.Interfaces;

public interface IExpenseService
{
    List<Expense> GetAll(string userId);
    Expense? GetById(Guid id, string userId);
    void Create(Expense expense, string userId);
    void Update(Expense expense, string userId);
    void Delete(Guid id, string userId);
    List<Expense> GetByDateRange(DateTime start, DateTime end, string userId);
    List<Expense> GetByCategory(Guid categoryId, string userId);
    List<Expense> GetCurrentMonthExpenses(string userId);
    decimal GetTotalForMonth(int month, int year, string userId);
    Dictionary<Guid, decimal> GetTotalsByCategory(int month, int year, string userId);
    List<Expense> GetRecent(int count, string userId);
}
