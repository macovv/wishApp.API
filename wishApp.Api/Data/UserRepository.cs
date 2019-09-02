using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wishApp.Api.Models;

namespace wishApp.Api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _ctx;

        public UserRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }  

        public async Task<User> GetUser(string name)
        {
            var user = await _ctx.Users.Include(w => w.UserWishes).Where(u => u.UserName == name).FirstOrDefaultAsync();
            return user;
        }

        public IQueryable<User> GetUsers()
        {
            return _ctx.Users.Include(w => w.UserWishes).AsQueryable();
        }

        // public async Task<Wish> GetWish(int id)
        // {
        //     return await _ctx.Wishes.Where(i => i.Id == id).FirstOrDefaultAsync();
        // }

        // public async Task<IList<Wish>> GetWishes(string userName)
        // {
        //     return await _ctx.Wishes.ToListAsync();
        // }

        public async Task<bool> SaveAll()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}