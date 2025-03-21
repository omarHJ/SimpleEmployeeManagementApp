using Microsoft.AspNetCore.Mvc;
using SimpleEmployeeManagementApp.Models;
using SimpleEmployeeManagementApp.Services;
using Microsoft.AspNetCore.Http; // Add this for session access

namespace YourNamespace.Controllers // Replace YourNamespace with your actual namespace
{
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IHttpContextAccessor _httpContextAccessor; // Add this

        // Inject IHttpContextAccessor
        public AccountController(ILoginService loginService, IHttpContextAccessor httpContextAccessor)
        {
            _loginService = loginService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Account/Login
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] // Prevent caching
        public IActionResult Login()
        {
            //if the user tries to go to the login page while he is logged in, redirect him.
            if (_loginService.IsLoggedIn())
            {
                return RedirectToAction("Index", "Employee");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] // Prevent caching
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginService.LoginAsync(model);
                if (result)
                {
                    // Login successful, redirect to the Employee Index page
                    return RedirectToAction("Index", "Employee");
                }
                // If login fails, add a model error and return to the login view
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If ModelState is invalid or login failed, return to the login view with the model
            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            _loginService.Logout(); // Call the Logout method from your LoginService
            return RedirectToAction("Login", "Account"); // Redirect to the login page
        }
    }
}