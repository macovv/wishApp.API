using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wishApp.Api.Models
{
    public class Wish
    {
        public int Id { get; set; }
        [Required]
        public int Cost { get; set; }
        public int CollectedMoney { get; set; }
        public string UrlToShop { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Finished { get; set; }
        public bool IsFinished { get; set; } = false;
        [Required]
        public string WishDescription { get; set; }
        public User User { get; set; }
        //public IColl
    }
}