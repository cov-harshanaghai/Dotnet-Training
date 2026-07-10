using AutoMapper;
using Ecommerce_DBFirst.Dtos;
using Ecommerce_DBFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_DBFirst.Controllers
{
    [Route("/product")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly EcommerceDbfirstContext _context;
        private readonly IMapper _mapper;

        public ProductController(EcommerceDbfirstContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Route("index")]
        public IActionResult Index(bool sortByPrice, int? categoryId)
        {
            var products = _context.Products.AsQueryable();

            if (sortByPrice)
            {
                products = products.OrderBy(p => p.Price);
            }
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }
            ViewBag.CategoryOptions = _context.Categories.ToList();
            return View(products.ToList());

        }

        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string searchTerm)
        {

            var products = _context.Products.AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {

                products = products.Where(p => p.ProductName.Contains(searchTerm));
            }
            return PartialView("_ProductListPartial", products.ToList());
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
            Product mappedProduct = _mapper.Map<Product>(product);
            var duplicate = _context.Products
        .FirstOrDefault(p => p.ProductName.ToLower() == product.ProductName.ToLower());

            if (duplicate != null)
            {
                TempData["ErrorMessage"] = "Product already exists";

                ViewBag.CategoryId = new SelectList(
                    _context.Categories,
                    "CategoryId",
                    "CategoryName",
                    product.CategoryId);
                return View(product);
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(mappedProduct);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product added successfully";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(
                _context.Categories,
                "CategoryId",
                "CategoryName",
                product.CategoryId);
            return View(product);
        }

        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpGet]
        [Route("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            ViewBag.ProductId = id;   // <-- add this line

            ViewBag.CategoryId = new SelectList(
                _context.Categories,
                "CategoryId",
                "CategoryName",
                product.CategoryId);
            return View(_mapper.Map<ProductUpdateDto>(product));
        }

        [Route("edit/{id}")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, ProductUpdateDto updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = id;   // <-- add this line

                ViewBag.CategoryId = new SelectList(
                    _context.Categories,
                    "CategoryId",
                    "CategoryName",
                    updatedProduct.CategoryId);

                return View(updatedProduct);
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            _mapper.Map(updatedProduct, product);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
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