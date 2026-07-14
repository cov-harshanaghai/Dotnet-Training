using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_DBFirst.Models;
using System.Diagnostics;

namespace Ecommerce_DBFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Home/Error")]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error,
                    "Unhandled exception occurred at path {Path}.", exceptionFeature.Path);
            }

            return View(model);
        }

        [Route("Home/StatusCode/{code}")]
        public IActionResult StatusCodeHandler(int code)
        {
            _logger.LogWarning("Status code {StatusCode} triggered for path {Path}.",
                code, HttpContext.Request.Path);

            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = code
            };

            switch (code)
            {
                case 404:
                    model.Title = "Page Not Found";
                    model.Message = "The page you're looking for doesn't exist or may have been moved.";
                    break;
                case 403:
                    model.Title = "Access Denied";
                    model.Message = "You don't have permission to view this page.";
                    break;
                default:
                    model.Title = "Something Went Wrong";
                    model.Message = "An unexpected error occurred while processing your request.";
                    break;
            }

            return View("Error", model);
        }
    }
}