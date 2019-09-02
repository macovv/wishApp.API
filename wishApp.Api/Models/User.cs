using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace wishApp.Api.Models
{
    public class User : IdentityUser<int>
    {
        public int Income { get; set; } // like tip
        public int Costs { get; set; } = 0;
        public int Saldo { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ICollection<Wish> UserWishes { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        //ICollection of products which client want to buy(Wishes MODEL)
    }
}