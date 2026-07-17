using Ecommerce_DBFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_DBFirst.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommerceDbfirstContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(EcommerceDbfirstContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;

        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Fetching all categories from the database.");
            return await _context.Categories.ToListAsync();
        }
        public async Task<bool> CategoryNameExistsAsync(string categoryName)
        {
            _logger.LogInformation("Checking if category name exists: {CategoryName}", categoryName);
            return await _context.Categories.AnyAsync(c => c.CategoryName == categoryName);
        }
        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Category {CategoryName} added successfully.", category.CategoryName);
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Category {CategoryName} updated successfully.",
                category.CategoryName);
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                _logger.LogWarning(
                    "Category with ID {CategoryId} not found.",
                    id);
            }

            return category;
        }
        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Category {CategoryName} deleted successfully.",
                category.CategoryName);
        }
    }
}

