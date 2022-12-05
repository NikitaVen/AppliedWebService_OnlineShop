using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
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
            return View(context.Items as ICollection<Item>);
        }
    }
}
