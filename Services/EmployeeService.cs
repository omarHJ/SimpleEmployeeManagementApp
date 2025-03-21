using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SimpleEmployeeManagementApp.Data;
using SimpleEmployeeManagementApp.Models;
using SimpleEmployeeManagementApp.Services;

namespace SimpleEmployeeManagementApp.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.FromSqlRaw("SELECT * FROM GetAllEmployees").ToListAsync();
        }
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            var parameters = new[]
            {
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Position", employee.Position),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@Salary", employee.Salary),
                new SqlParameter("@DateOfJoining", employee.DateOfJoining),
                new SqlParameter("@IsActive", employee.IsActive)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC AddEmployee @FirstName, @LastName, @Position, @Email, @Salary, @DateOfJoining, @IsActive", parameters);

            return employee;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var parameter = new SqlParameter("@Id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC suspendEmployee @Id", parameter);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", employee.Id),
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Position", employee.Position),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@Salary", employee.Salary),
                new SqlParameter("@DateOfJoining", employee.DateOfJoining),
                new SqlParameter("@IsActive", true)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateEmployee @Id, @FirstName, @LastName, @Position, @Email, @Salary, @DateOfJoining, @IsActive", parameters);
        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var parameter = new SqlParameter("@Id", id);
            var employee = await _context.Employees
                .FromSqlRaw("EXEC GetEmployeeById @Id", parameter)
                .ToListAsync();

            return employee.FirstOrDefault();
        }
    }
}

