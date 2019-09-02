using System;
namespace wishApp.Api.Models
{
    public class UserWish
    {
      
        public int UserId { get; set; }
        public int WishId { get; set; }
        public User User { get; set; }
        public Wish Wish { get; set; }

    }
}
