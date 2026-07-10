using Microsoft.AspNetCore.Identity;

namespace Ecommerce_DBFirst.Models
{
    public class AppUser : IdentityUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        
        public string? ShippingAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }

        
        public string? Role { get; set; } = "Customer";

    }
}
