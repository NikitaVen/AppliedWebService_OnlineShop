using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineShop.Models;
using System.Web;
using System.Collections.Generic;
using System.Text.Json;

namespace OnlineShop.Controllers
{
    public class ShopController : Controller
    {
        ShopContext context;
       // IHttpContextAccessor accessor;
      //  Basket basket;

        public ShopController(ShopContext context)//, IHttpContextAccessor accessor)//, Basket basket)
        {
            this.context = context;
            //this.accessor = accessor;
            //     this.basket = basket;
        }

        [HttpGet]
        public IActionResult Table()
        {
            return View(context.Items.ToList<Item>());
        }

        public IActionResult CartAdd(string button)
        {
            string value;
            HttpContext.Request.Cookies.TryGetValue("Cart", out value);
          //  var value = HttpContext.Session.GetString("Cart");
            Basket basket;
            if (value == null)
                basket = new Basket();
            else
                basket = JsonSerializer.Deserialize<Basket>(value);

            BasketAction(basket, button);
            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize<Basket>(basket));
          //  HttpContext.Session.SetString("Cart", JsonSerializer.Serialize<Basket>(basket));
            return RedirectToAction("Table");
        }

        private void BasketAction(Basket basket, string button)
        {
            Item chosenItem = context.Items.First(i => i.Id == Int64.Parse(button));
            int amount;
            if (basket.items.TryGetValue(chosenItem.Id, out amount))
            {
                if (amount < chosenItem.Amount)
                    basket.items[chosenItem.Id] = amount + 1;
            }
            else
                basket.items.Add(chosenItem.Id, 1);
        }

        [HttpGet]
        public IActionResult Item(long itemId)
        {
            return View(context.Items.First(i=>i.Id == itemId));
        }

        [HttpGet]
        public IActionResult Cart()
        {
            string value;
            Basket basket;
            HttpContext.Request.Cookies.TryGetValue("Cart", out value);
            if (value == null)
                basket = new Basket();
            else
                basket = JsonSerializer.Deserialize<Basket>(value);

            Cart cart = new Cart();

            foreach(var id in basket.items.Keys)
            {
                Item item = context.Items.First(i => i.Id == id);
                cart.items.Add(item, basket.items[id]);
            }

            return View(cart);
        }

        public IActionResult CartDelete(string button)
        {
            string value;
            HttpContext.Request.Cookies.TryGetValue("Cart", out value);

            Basket basket;
            if (value == null)
                basket = new Basket();
            else
                basket = JsonSerializer.Deserialize<Basket>(value);

            basket.items.Remove(Int64.Parse(button));

            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize<Basket>(basket));
            return RedirectToAction("Cart");
        }

        public IActionResult CartDecrement(string dec)
        {
            string value;
            HttpContext.Request.Cookies.TryGetValue("Cart", out value);

            Basket basket;
            if (value == null)
                basket = new Basket();
            else
                basket = JsonSerializer.Deserialize<Basket>(value);
            if (basket.items[Int64.Parse(dec)] > 1)
                basket.items[Int64.Parse(dec)] -= 1;

            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize<Basket>(basket));
            return RedirectToAction("Cart");
        }

        public IActionResult CartIncrement(string inc)
        {
            string value;
            HttpContext.Request.Cookies.TryGetValue("Cart", out value);

            Basket basket;
            if (value == null)
                basket = new Basket();
            else
                basket = JsonSerializer.Deserialize<Basket>(value);
            if (basket.items[Int64.Parse(inc)] < context.Items.First(i => i.Id == Int64.Parse(inc)).Amount)
                basket.items[Int64.Parse(inc)] += 1;

            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize<Basket>(basket));
            return RedirectToAction("Cart");
        }
        
        [HttpPost]
        public IActionResult GoToOrderCreation()
        {
            return RedirectToAction("Order"); 
        }

        [HttpGet]
        public IActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            string value;
            HttpContext.Request.Cookies.TryGetValue("Cart", out value);
            Basket basket;
            if (value == null)
            {
                return RedirectToAction("Cart");   
            }

            basket = JsonSerializer.Deserialize<Basket>(value);
            
            Console.WriteLine(order.Id);
            order.OrderItems = new List<OrderItem>();
            foreach(KeyValuePair<long, int> entry in basket.items)
            {
                OrderItem oi = new OrderItem();
                oi.Item = context.Items.First(i => i.Id == entry.Key);
                oi.Order = order;
                oi.Amount = entry.Value;
                order.OrderItems.Add(oi);
            }
            order.Order_date = DateTime.Now;
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return RedirectToAction("Table");
        }
    }
}
