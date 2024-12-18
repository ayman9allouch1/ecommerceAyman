// Controllers/ProductController.cs
using ecommerceAyman.Models;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ecommerceAyman.Controllers
{
    public class ProductController : Controller
    {
        private readonly ECommerceDbContext _context;

        public ProductController(ECommerceDbContext context)
        {
            _context = context;
        }

        // GET: Product Page (Display Products)
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            // Retrieve user information based on the userId
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);

            // Retrieve the list of products
            var products = _context.Products.ToList();

            // Pass both user and products to the view
            ViewBag.UserName = user?.UserName;  // Pass the user's name to the view

            return View(products);
        }



        // POST: Add to Cart
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId != null)
            {
                var userCart = new UserCart
                {
                    UserID = (int)userId,
                    ProductID = productId,
                    Quantity = 1,
                    DateAdded = DateTime.Now
                };

                _context.UserCarts.Add(userCart);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Show Cart
        // Show Cart View
        public IActionResult ShowCart()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);

            // Retrieve the list of products
            var products = _context.Products.ToList();

            // Pass both user and products to the view
            ViewBag.UserName = user?.UserName;
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var cartItems = _context.UserCarts
                .Where(uc => uc.UserID == userId)
                .Include(uc => uc.Product) // Include product details
                .ToList();

            return View(cartItems);
        }


        // POST: Delete from Cart
        [HttpPost]
        public IActionResult RemoveFromCart(int userCartId)
        {
            var cartItem = _context.UserCarts.FirstOrDefault(uc => uc.UserCartID == userCartId);
            if (cartItem != null)
            {
                _context.UserCarts.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }
    }
}
