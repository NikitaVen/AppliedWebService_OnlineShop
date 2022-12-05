using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using System.Drawing.Imaging;

namespace OnlineShop.Controllers
{
    public class ShopController : Controller
    {
        ShopContext context;
        Basket basket;

        public ShopController(ShopContext context)
        {
            this.context = context;
            this.basket = new Basket();
        }

        [HttpGet]
        public IActionResult Table()
        {
            return View(context.Items.ToList<Item>());
        }

        public void defaultBasketPut(string itemId)
        {
            Item chosenItem = context.Items.First(i => i.Id == Int64.Parse(itemId));
            if (chosenItem.Amount > 0)
            {
                int amount;
                if (basket.items.TryGetValue(chosenItem, out amount))
                {
                    basket.items.Add(chosenItem, amount + 1);
                }
                else
                    basket.items.Add(chosenItem, 1);
            }
        }
    }
}
