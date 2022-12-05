using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
namespace OnlineShop.Controllers
{
    public class ShopController : Controller
    {
        ShopContext context = new ShopContext();

        [HttpGet]
        public IActionResult Table()
        {
            return View(context.Items);
        }
    }
}
