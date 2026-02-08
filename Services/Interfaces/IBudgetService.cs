using Pratice.Models;

namespace Pratice.Services.Interfaces;

public interface IBudgetService
{
    List<Budget> GetAll(string userId);
    List<Budget> GetForMonth(int month, int year, string userId);
    Budget? GetById(Guid id, string userId);
    void Create(Budget budget, string userId);
    void Update(Budget budget, string userId);
    void Delete(Guid id, string userId);
    void RecalculateSpentAmounts(int month, int year, string userId);
    List<Budget> GetBudgetsWithAlerts(int month, int year, string userId);
    Budget? GetByCategoryAndMonth(Guid categoryId, int month, int year, string userId);
}
