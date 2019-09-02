using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using wishApp.Api.Models;

namespace wishApp.Api.Dtos
{
    public class UserForRegisterDto
    {
        // public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}