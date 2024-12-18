using Microsoft.AspNetCore.Mvc;

namespace ecommerceAyman.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
