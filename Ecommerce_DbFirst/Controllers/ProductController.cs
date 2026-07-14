using AutoMapper;
using Ecommerce_DBFirst.Dtos;
using Ecommerce_DBFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ecommerce_DBFirst.Services;

namespace Ecommerce_DBFirst.Controllers
{
    [Route("/product")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly EcommerceDbfirstContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(EcommerceDbfirstContext context, IMapper mapper, ILogger<ProductController> logger, IProductService productService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _productService = productService;
        }

        [Route("index")]
        public IActionResult Index(bool sortByPrice, int? categoryId)
        {
            
            var products = _productService.GetAllProducts(sortByPrice, categoryId);
            ViewBag.CategoryOptions = _context.Categories.ToList();
            return View(products);
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string searchTerm)
        {
            var products = _productService.SearchProducts(searchTerm);
            return PartialView("_ProductListPartial", products);
        }

        [Route("add")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            ViewBag.CategoryId = new SelectList(
                _context.Categories,
                "CategoryId",
                "CategoryName");
            return View();
        }

        [Route("add")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(ProductAddDto product)
        {
            _logger.LogInformation("Admin submitted add product form for {ProductName}.", product.ProductName);

            if (_productService.ProductNameExists(product.ProductName))
            {
                _logger.LogWarning("Duplicate product attempted: {ProductName}.", product.ProductName);
                TempData["ErrorMessage"] = "Product already exists";
                ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }

            try
            {
                var mappedProduct = _mapper.Map<Product>(product);
                _productService.AddProduct(mappedProduct);
                TempData["SuccessMessage"] = "Product added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure while adding product {ProductName}.", product.ProductName);
                TempData["ErrorMessage"] = "An error occurred while adding the product.";
                ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
        }

        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpGet]
        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            ViewBag.ProductId = id;
            ViewBag.CategoryId = new SelectList(
                _context.Categories,
                "CategoryId",
                "CategoryName",
                product.CategoryId);
            return View(_mapper.Map<ProductUpdateDto>(product));
        }

        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, ProductUpdateDto updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = id;
                ViewBag.CategoryId = new SelectList(
                    _context.Categories,
                    "CategoryId",
                    "CategoryName",
                    updatedProduct.CategoryId);
                return View(updatedProduct);
            }

            var product = _mapper.Map<Product>(updatedProduct);
            product.ProductId = id;

            _productService.UpdateProduct(product);
            return RedirectToAction(nameof(Index));
        }

        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            _productService.DeleteProduct(product);
            return RedirectToAction("Index");
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            Order order = new Order();
            order.OrderDate = DateOnly.FromDateTime(DateTime.Now);
            order.TotalAmount = (int)product.Price;
            _context.Orders.Add(order);
            _context.SaveChanges();
            return View();
        }
    }
}