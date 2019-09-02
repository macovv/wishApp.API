using System.ComponentModel.DataAnnotations;
namespace wishApp.Api.Dtos
{
    public class UserForLoginDto
    {
        // public int Id { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember ME")]
        public bool RememberMe { get; set; }
    }
}