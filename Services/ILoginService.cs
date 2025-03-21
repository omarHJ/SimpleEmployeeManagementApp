using SimpleEmployeeManagementApp.Models;
using System.Threading.Tasks;

namespace SimpleEmployeeManagementApp.Services
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(LoginViewModel model);
        Task<bool> UserExistsAsync(string email); 
        bool IsLoggedIn();
        void Logout();
    }
}
