using Microsoft.EntityFrameworkCore;
using SimpleEmployeeManagementApp.Data;
using SimpleEmployeeManagementApp.Models;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Http; // Add this

namespace SimpleEmployeeManagementApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; // Add this

        public LoginService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);

            if (user == null)
            {
                return false;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (isValidPassword)
            {
                // Set a session variable to indicate successful login.
                _httpContextAccessor.HttpContext.Session.SetString("IsLoggedIn", "true");
                _httpContextAccessor.HttpContext.Session.SetInt32("UserId", user.Id); // Store UserId, you'll need it later on.
                return true;
            }

            return false;
        }

        public void Logout()
        {
            // Clear the session variable.
            _httpContextAccessor.HttpContext.Session.Clear(); // Clear the entire session
        }

        // Add a method to check if the user is logged in
        public bool IsLoggedIn()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("IsLoggedIn") == "true";
        }

        //get logged in user id.
        public int? GetLoggedInUserId()
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
        }
    }
}