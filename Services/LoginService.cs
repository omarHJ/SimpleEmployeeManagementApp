using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SimpleEmployeeManagementApp.Data;
using SimpleEmployeeManagementApp.Models;
using System.Threading.Tasks;

namespace SimpleEmployeeManagementApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            // Declare the output parameter to hold the return value
            var returnValue = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            // Define parameters for the stored procedure
            var parameters = new[]
            {
                new SqlParameter("@UserName", model.Username),
                new SqlParameter("@Password", model.Password),
                returnValue
            };

            // Execute the stored procedure
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AuthenticateUser @UserName, @Password, @ReturnValue OUTPUT",
                parameters
            );

            // Get the result from the output parameter
            var result = (int)returnValue.Value;

            // Return true if the result is 1 (valid login), otherwise false
            return result == 1;
        }
    }
}
