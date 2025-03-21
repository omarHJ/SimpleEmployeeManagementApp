using Microsoft.AspNetCore.Mvc;
using SimpleEmployeeManagementApp.Models;
using SimpleEmployeeManagementApp.Services;
using Microsoft.AspNetCore.Http;

namespace SimpleEmployeeManagementApp.Controllers 
{
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        
        public AccountController(ILoginService loginService, IHttpContextAccessor httpContextAccessor)
        {
            _loginService = loginService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] 
        public IActionResult Login()
        {
            if (_loginService.IsLoggedIn())
            {
                return RedirectToAction("Index", "Employee");
            }
            return View();
        }

        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] 
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _loginService.UserExistsAsync(model.Email);
                if (userExists)
                {
                    var result = await _loginService.LoginAsync(model);
                    if (result)
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                    TempData["ErrorMessage"] = "Wrong email or password.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid login attempt.";
                }
            }

            return View(model);
        }

        
        public IActionResult Logout()
        {
            _loginService.Logout(); 
            return RedirectToAction("Login", "Account"); 
        }
    }
}