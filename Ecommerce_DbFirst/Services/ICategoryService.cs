using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task UpdateCategoryAsync(Category category);
        Task<bool> CategoryNameExistsAsync(string categoryName);
        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
