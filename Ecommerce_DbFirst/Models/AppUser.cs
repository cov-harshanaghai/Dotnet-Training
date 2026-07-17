using Microsoft.AspNetCore.Identity;

namespace Ecommerce_DBFirst.Models
{
    public class AppUser : IdentityUser
    {

        public string? FirstName { get; set; } 
        public string? LastName { get; set; }  = null;

        
        public string? ShippingAddress { get; set; } = String.Empty;
        public string? City { get; set; }= string.Empty;
        public string? PostalCode { get; set; } = string.Empty;

    }
}
