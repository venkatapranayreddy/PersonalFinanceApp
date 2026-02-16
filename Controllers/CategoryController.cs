using Microsoft.AspNetCore.Mvc;
using Pratice.Services.Interfaces;

namespace Pratice.Controllers;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateExpenseCategory([FromForm] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Json(new { success = false, error = "Category name is required." });
        }

        if (name.Trim().Length > 50)
        {
            return Json(new { success = false, error = "Category name must be 50 characters or less." });
        }

        var existing = _categoryService.GetExpenseCategories(UserId);
        if (existing.Any(c => c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            return Json(new { success = false, error = "A category with this name already exists." });
        }

        var category = _categoryService.CreateExpenseCategory(name, UserId);
        return Json(new { success = true, id = category.Id, name = category.Name });
    }
}
