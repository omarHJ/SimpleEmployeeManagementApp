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

        // GET: /Employee/Create
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Create()
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: /Employee/Create
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
                return RedirectToAction(nameof(Index)); // Redirect to the list after creation
            }
            return View(employee); // Return to the form with errors
        }

        // GET: /Employee/Edit/5
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

        // POST: /Employee/Edit/5
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
                return BadRequest(); // Or NotFound(), depending on your preference
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.UpdateEmployeeAsync(employee);
                }
                catch (DbUpdateConcurrencyException) // Handle concurrency conflicts
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

        // GET: /Employee/Delete/5
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

            return View(employee); // Show a confirmation view
        }

        // POST: /Employee/Delete/5
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