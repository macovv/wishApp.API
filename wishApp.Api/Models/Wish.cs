using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wishApp.Api.Models
{
    public class Wish
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int CollectedMoney { get; set; }
        public string UrlToShop { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Finished { get; set; }
        public bool IsFinished { get; set; } = false;
        public string WishDescription { get; set; }
        public User User { get; set; }
        //public IColl
    }
}