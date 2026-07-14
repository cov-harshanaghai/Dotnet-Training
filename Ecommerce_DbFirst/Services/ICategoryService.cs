using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst.Services
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        Category? GetCategoryById(int id);
        void UpdateCategory(Category category);
        bool CategoryNameExists(string categoryName);
        void AddCategory(Category category);
        void DeleteCategory(Category category);
    }
}
