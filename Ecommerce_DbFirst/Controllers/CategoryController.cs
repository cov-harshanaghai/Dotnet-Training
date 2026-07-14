using Ecommerce_DBFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_DBFirst.Services;

namespace Ecommerce_DBFirst.Controllers
{
    [Route("/Category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var categories = _categoryService.GetAllCategories();
            return View(categories);
        }

        [Route("add")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Category category)
        {
            _logger.LogInformation("Adding a new category: {CategoryName}", category.CategoryName);

            if (_categoryService.CategoryNameExists(category.CategoryName))
            {
                _logger.LogWarning("Category name already exists: {CategoryName}", category.CategoryName);
                ModelState.AddModelError("CategoryName", "Category name already exists.");
                return View(category);
            }

            try
            {
                _categoryService.AddCategory(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding category: {CategoryName}", category.CategoryName);
                ModelState.AddModelError(string.Empty, "An error occurred while adding the category. Please try again.");
                return View(category);
            }
        }

        [HttpGet]
        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            _categoryService.UpdateCategory(category);
            return RedirectToAction("Index");
        }

        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null) return NotFound();

            _categoryService.DeleteCategory(category);
            return RedirectToAction("Index");
        }
    }
}