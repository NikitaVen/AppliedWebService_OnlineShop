using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using System.Drawing.Imaging;

namespace OnlineShop.Controllers
{
    public class ShopController : Controller
    {
        ShopContext context;

        public ShopController(ShopContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Table()
        {
            return View(context.Items.ToList<Item>());
        }
    }
}
