using SimpleEmployeeManagementApp.Models;

namespace SimpleEmployeeManagementApp.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task<Employee> GetEmployeeByIdAsync(int id); 
    }
}
