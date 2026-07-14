using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommerceDbfirstContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(EcommerceDbfirstContext context , ILogger<CategoryService>logger)
        {
            _context = context;
            _logger = logger;
            
        }
        public List<Category> GetAllCategories()
        {
            _logger.LogInformation("Fetching all categories from the database.");
            return _context.Categories.ToList();
        }
        public bool CategoryNameExists(string categoryName)
        {
            _logger.LogInformation("Checking if category name exists: {CategoryName}", categoryName);
            return _context.Categories.Any(c => c.CategoryName == categoryName);
        }
        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _logger.LogInformation("Category {CategoryName} added successfully.", category.CategoryName);
            _context.SaveChanges();
        }
        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _logger.LogInformation("Category {CategoryName} updated successfully.", category.CategoryName);
            _context.SaveChanges();
        }
        public Category GetCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found.", id);
            }
            return category;
        }
        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            _logger.LogInformation("Category {CategoryName} deleted successfully.", category.CategoryName);
            _context.SaveChanges();
        }
    }
}
