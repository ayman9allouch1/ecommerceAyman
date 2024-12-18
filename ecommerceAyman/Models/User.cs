// Models/User.cs
namespace ecommerceAyman.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Store hashed password
        public DateTime DateCreated { get; set; }
    }
}
