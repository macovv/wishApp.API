using System.Collections.Generic;
using System.Threading.Tasks;
using wishApp.Api.Models;

namespace wishApp.Api.Data
{
    public interface IWishRepository
    {
        Task<bool> SaveAll();
        Task<IList<Wish>> GetWishes();
        Task<Wish> GetWish(int id);
        Task<IList<Wish>> GetUserWishes(string userName);
        void AddWish(User user, Wish wishToAdd);
        void RemoveWish(Wish wishToRemove);
    }
}