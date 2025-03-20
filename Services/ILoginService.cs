using SimpleEmployeeManagementApp.Models;
using System.Threading.Tasks;

namespace SimpleEmployeeManagementApp.Services
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(LoginViewModel model);
    }
}
