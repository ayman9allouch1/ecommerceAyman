// If you already have this file, there's no need to create it again.
using Microsoft.AspNetCore.Identity;

namespace ecommerceAyman.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
