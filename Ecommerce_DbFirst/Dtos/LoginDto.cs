using System.ComponentModel.DataAnnotations;

namespace Ecommerce_DBFirst.Dtos
{
    public class LoginDto
    {

        [Required(ErrorMessage = "Email is required")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public Boolean RememberMe { get; set; }

    }
}
