using Microsoft.EntityFrameworkCore;
using SimpleEmployeeManagementApp.Data;
using SimpleEmployeeManagementApp.Models;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;

namespace SimpleEmployeeManagementApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public LoginService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                return false;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (isValidPassword)
            {
                _httpContextAccessor.HttpContext.Session.SetString("IsLoggedIn", "true");
                //_httpContextAccessor.HttpContext.Session.SetInt32("UserId", user.Id);
                return true;
            }

            return false;
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public bool IsLoggedIn()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("IsLoggedIn") == "true";
        }
        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}