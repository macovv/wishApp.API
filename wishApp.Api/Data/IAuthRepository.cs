using System.Threading.Tasks;
using wishApp.Api.Models;

namespace wishApp.Api.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password, bool rememberMe, bool lockoutOnFailure);
         Task<bool> UserExist(string username);
    }
}