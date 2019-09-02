using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishApp.Api.Models;

namespace wishApp.Api.Data
{
    public interface IUserRepository
    {
        Task<User> GetUser(string name);
        IQueryable<User> GetUsers(); // list?
        Task<bool> SaveAll();
    }
}