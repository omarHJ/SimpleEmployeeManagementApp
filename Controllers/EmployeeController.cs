using Microsoft.AspNetCore.Mvc;
using SimpleEmployeeManagementApp.Services;

namespace SimpleEmployeeManagementApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILoginService _loginService;

        public EmployeeController(IEmployeeService employeeService, ILoginService loginService)
        {
            _employeeService = employeeService;
            _loginService = loginService;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] // Add this!
        public async Task<IActionResult> Index()
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not logged in
            }
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        // Add [ResponseCache] to any other actions that should not be cached.
        // For example, if you have an "Edit" action:
        // [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        // public IActionResult Edit(int id) { ... }
    }
}