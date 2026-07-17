using AutoMapper;
using Ecommerce_DBFirst.Dtos;
using Ecommerce_DBFirst.Models;
using Ecommerce_DBFirst.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_DBFirst.Controllers
{
    [Route("/product")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
  


        public ProductController(
            IMapper mapper,
            ILogger<ProductController> logger,
            IProductService productService,
            ICategoryService categoryService
           )
        {
            _mapper = mapper;
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
          
        }


        private async Task LoadCategoriesAsync(int? selectedCategoryId = null)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.CategoryId = new SelectList(
                categories,
                "CategoryId",
                "CategoryName",
                selectedCategoryId);
        }


        [Route("index")]
        public async Task<IActionResult> Index(ProductFilter filter)
        {
            var products = await _productService.GetAllProductsAsync(filter);

            await LoadCategoriesAsync();

            return View(products);
        }


        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = await _productService.SearchProductsAsync(searchTerm);

            return PartialView("_ProductListPartial", products);
        }


        [HttpGet]
        [Route("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            await LoadCategoriesAsync();

            return View();
        }


        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(ProductDto product)
        {
            _logger.LogInformation(
                "Admin submitted add product form for {ProductName}.",
                product.ProductName);


            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(product.CategoryId);
                return View(product);
            }


            if (await _productService.ProductNameExistsAsync(product.ProductName))
            {
                TempData["ErrorMessage"] = "Product already exists";

                await LoadCategoriesAsync(product.CategoryId);

                return View(product);
            }


            try
            {
                var mappedProduct = _mapper.Map<Product>(product);

                await _productService.AddProductAsync(mappedProduct);

                TempData["SuccessMessage"] =
                    "Product added successfully";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Failure while adding product {ProductName}.",
                    product.ProductName);


                TempData["ErrorMessage"] =
                    "An error occurred while adding the product.";

                await LoadCategoriesAsync(product.CategoryId);

                return View(product);
            }
        }



        [HttpGet]
        [Route("detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var category = await _categoryService.GetCategoryByIdAsync(product.CategoryId);
            ViewBag.CategoryName = category?.CategoryName ?? "Uncategorized";

            return View(product);
        }



        [HttpGet]
        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();


            await LoadCategoriesAsync(product.CategoryId);
            ViewBag.ProductId = product.ProductId;


            return View(_mapper.Map<ProductDto>(product));
        }



        [HttpPost]
        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            int id,
            ProductDto updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(updatedProduct.CategoryId);
                ViewBag.ProductId = id;

                return View(updatedProduct);
            }


            var product = await _productService.GetProductByIdAsync(id);


            if (product == null)
                return NotFound();


            _mapper.Map(updatedProduct, product);


            await _productService.UpdateProductAsync(product);


            TempData["SuccessMessage"] =
                "Product updated successfully";


            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();


            await _productService.DeleteProductAsync(product);


            return RedirectToAction(nameof(Index));
        }



        [Route("buy/{id}")]
        public async Task<IActionResult> Buy(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();


            Order order = new Order
            {
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                TotalAmount = product.Price
            };

            

            return View(product);
        }
    }
}