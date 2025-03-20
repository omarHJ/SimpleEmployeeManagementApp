//using Microsoft.AspNetCore.Mvc;
//using SimpleEmployeeManagementApp.Models;
//using SimpleEmployeeManagementApp.Services;

//namespace SimpleEmployeeManagementApp.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly ILoginService _loginService;

//        public AccountController(ILoginService loginService)
//        {
//            _loginService = loginService;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            var result = await _loginService.LoginAsync(model);
//            if (result)
//            {
//                return RedirectToAction("Index", "Employee");
//            }
//            return View(model);
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using SimpleEmployeeManagementApp.Models;
using SimpleEmployeeManagementApp.Services;

namespace YourNamespace.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginService.LoginAsync(model);
                if (result)
                {
                    return RedirectToAction("Index", "Employee");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }
    }
}
