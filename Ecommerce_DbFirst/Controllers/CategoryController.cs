using Ecommerce_DBFirst.Models;
using Ecommerce_DBFirst.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_DBFirst.Controllers
{
    [Route("/Category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;


        public CategoryController(
            ILogger<CategoryController> logger,
            ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }


        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

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
        public async Task<IActionResult> Add(Category category)
        {
            _logger.LogInformation(
                "Adding a new category: {CategoryName}",
                category.CategoryName);


            if (!ModelState.IsValid)
            {
                return View(category);
            }


            if (await _categoryService.CategoryNameExistsAsync(category.CategoryName))
            {
                _logger.LogWarning(
                    "Category name already exists: {CategoryName}",
                    category.CategoryName);


                ModelState.AddModelError(
                    "CategoryName",
                    "Category name already exists.");


                return View(category);
            }


            try
            {
                await _categoryService.AddCategoryAsync(category);


                TempData["SuccessMessage"] =
                    "Category added successfully";


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while adding category: {CategoryName}",
                    category.CategoryName);


                ModelState.AddModelError(
                    string.Empty,
                    "An error occurred while adding the category. Please try again.");


                return View(category);
            }
        }




        [HttpGet]
        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);


            if (category == null)
                return NotFound();


            return View(category);
        }




        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return View(category);
            }


            await _categoryService.UpdateCategoryAsync(category);


            TempData["SuccessMessage"] =
                "Category updated successfully";


            return RedirectToAction(nameof(Index));
        }




        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);


            if (category == null)
                return NotFound();


            await _categoryService.DeleteCategoryAsync(category);


            TempData["SuccessMessage"] =
                "Category deleted successfully";


            return RedirectToAction(nameof(Index));
        }
    }
}