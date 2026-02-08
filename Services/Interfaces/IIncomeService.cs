using Pratice.Models;

namespace Pratice.Services.Interfaces;

public interface IIncomeService
{
    List<Income> GetAll(string userId);
    Income? GetById(Guid id, string userId);
    void Create(Income income, string userId);
    void Update(Income income, string userId);
    void Delete(Guid id, string userId);
    List<Income> GetByDateRange(DateTime start, DateTime end, string userId);
    List<Income> GetCurrentMonthIncomes(string userId);
    decimal GetTotalForMonth(int month, int year, string userId);
    List<Income> GetRecent(int count, string userId);
}
