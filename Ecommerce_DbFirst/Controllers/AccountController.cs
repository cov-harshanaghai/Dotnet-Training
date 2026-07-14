using Ecommerce_DBFirst.Dtos;
using Ecommerce_DBFirst.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_DBFirst.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new AppUser()
                {
                    Email = user.Email,
                    UserName = user.Email,


                    FirstName = "New",
                    LastName = "User",
                    City = "Default",
                    ShippingAddress = "Default",
                    PostalCode = "00000"

                };
                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)

{
    await _userManager.AddToRoleAsync(newUser, "User");
    await _signInManager.SignInAsync(newUser, isPersistent: false);
    return RedirectToAction("Index", "Home");
}

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (ModelState.IsValid)
            {
                var res =  await _signInManager.PasswordSignInAsync(
                    user.Email, user.Password, user.RememberMe, lockoutOnFailure: false
                    );
                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                
                        ModelState.AddModelError("", "Invalid login attempt.");
                    
                }
                
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");

            
        }
        [HttpGet]
public IActionResult AccessDenied()
{
    return View();
}
    }
}
