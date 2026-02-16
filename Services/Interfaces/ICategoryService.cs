using Microsoft.AspNetCore.Mvc.Rendering;
using Pratice.Models;

namespace Pratice.Services.Interfaces;

public interface ICategoryService
{
    List<Category> GetExpenseCategories(string userId);
    List<Category> GetIncomeCategories(string userId);
    List<Category> GetAll(string userId);
    Category? GetById(Guid id, string userId);
    SelectList GetExpenseCategorySelectList(string userId, Guid? selectedId = null);
    SelectList GetIncomeCategorySelectList(string userId, Guid? selectedId = null);
    Category CreateExpenseCategory(string name, string userId);
}
