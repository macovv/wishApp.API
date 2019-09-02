using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wishApp.Api.Models;

namespace wishApp.Api.Data
{
    public class WishRepository : IWishRepository
    {
        private readonly AppDbContext _ctx;

        public WishRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IList<Wish>> GetUserWishes(string userName)
        {
            return await _ctx.Wishes.Where(u => u.User.UserName == userName).ToListAsync();
        }

        public async Task<Wish> GetWish(int id)
        {
            return await _ctx.Wishes.Include(u => u.User).Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Wish>> GetWishes()
        {
            return await _ctx.Wishes.ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

        public void RemoveWish(Wish wishToRemove) 
        {
            _ctx.Remove(wishToRemove);
        }

        public void AddWish(User user, Wish wishToAdd)
        {
            user.UserWishes.Add(wishToAdd);
        }
    }
}