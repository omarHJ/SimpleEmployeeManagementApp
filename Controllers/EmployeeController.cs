using Microsoft.AspNetCore.Mvc;
using SimpleEmployeeManagementApp.Services;
using SimpleEmployeeManagementApp.Models;
using Microsoft.EntityFrameworkCore;

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

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Index()
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Create()
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                await _employeeService.AddEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Edit(int id)
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            if (id != employee.Id)
            {
                return BadRequest(); 
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.UpdateEmployeeAsync(employee);
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (await _employeeService.GetEmployeeByIdAsync(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}