using Ecommerce_DBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecommerce_DBFirst.Controllers
{

    [Route("/Category")]
    public class CategoryController : Controller
    {
        private readonly EcommerceDbfirstContext _context;
        public CategoryController(EcommerceDbfirstContext context)
        {
            _context = context;
        }
        [Route("index")]
        //[Route("")]
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(category);
        }
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            return View(category);
        }

        [Route("edit/{id}")]
        [HttpPost]
        public IActionResult Edit(int id , Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var category1 = _context.Categories.FirstOrDefault(p => p.CategoryId == id);
            category1.CategoryName = category.CategoryName;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
   

    }
}
