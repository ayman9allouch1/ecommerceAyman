// Controllers/UserController.cs
using ecommerceAyman.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ecommerceAyman.Controllers
{
    public class UserController : Controller
    {
        private readonly ECommerceDbContext _context;

        public UserController(ECommerceDbContext context)
        {
            _context = context;
        }

        // GET: Login page
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    if (VerifyPasswordHash(model.Password, user.PasswordHash))
                    {
                        // Store user ID in session
                        HttpContext.Session.SetInt32("UserID", user.UserID);

                        // Redirect to products page
                        return RedirectToAction("Index", "Product");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return View(model);
        }
        
        // GET: Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return View(model);
                }

                // Check if email already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already taken.");
                    return View(model);
                }

                var passwordHash = HashPassword(model.Password);

                var newUser = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    DateCreated = DateTime.Now
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }
        // Hash the password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Verify the password hash
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
        public IActionResult Receipt()
        {
            var userId = HttpContext.Session.GetInt32("UserID"); // Get user ID from session

            if (userId == null)
            {
                return RedirectToAction("Login"); // Redirect to login if no user is logged in
            }

            // Fetch cart items for the user
            var userCartItems = _context.UserCarts
                                        .Where(uc => uc.UserID == userId)
                                        .Include(uc => uc.Product) // Include related Product data
                                        .ToList();

            var totalAmount = userCartItems.Sum(item => item.Quantity * item.Product.Price); // Calculate total

            // Create a ViewModel to pass data to the view
            var receiptViewModel = new ReceiptViewModel
            {
                CartItems = userCartItems,
                TotalAmount = totalAmount
            };

            return View(receiptViewModel); // Return the Receipt view with data
        }

    }
}
