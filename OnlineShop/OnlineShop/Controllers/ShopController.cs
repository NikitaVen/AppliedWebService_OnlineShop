using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using System.Text.Json;
using Google.Protobuf.Collections;
using Grpc.Net.Client;

namespace OnlineShop.Controllers
{
    public class ShopController : Controller
    {
        private OnlineShop.OnlineShopClient _client;

        public ShopController()
        {
            _client = new OnlineShop.OnlineShopClient(GrpcChannel.ForAddress("https://localhost:7208/"));
        }

        [HttpGet]
        public async Task<IActionResult> Table()
        {
            var reply = await _client.GetItemsAsync(new GetItemsRequest { ItemIds = { } });
            return View(JsonSerializer.Deserialize<List<Item>>(reply.Items));
        }

        public IActionResult CartAdd(string button)
        {
            string value;
            HttpContext.Request.Cookies.TryGetValue("Cart", out value);
            Basket basket;
            if (value == null)
                basket = new Basket();
            else
                basket = JsonSerializer.Deserialize<Basket>(value);

            BasketAction(basket, button);
            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize<Basket>(basket));
            return RedirectToAction("Table");
        }

        private void BasketAction(Basket basket, string button)
        {
            var reply = _client.GetItems(new GetItemsRequest { ItemIds = { Int64.Parse(button) } });
            Item chosenItem = JsonSerializer.Deserialize<List<Item>>(reply.Items)![0];
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
            var reply = _client.GetItems(new GetItemsRequest { ItemIds = { itemId } });
            return View(JsonSerializer.Deserialize<List<Item>>(reply.Items)![0]);
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
            RepeatedField<long> req = new RepeatedField<long>();
            if (basket.items.Keys.ToList().Count != 0)
            {
                req.AddRange(basket.items.Keys.ToList());
                var reply = _client.GetItems(new GetItemsRequest { ItemIds = req });
                List<Item> items = JsonSerializer.Deserialize<List<Item>>(reply.Items)!;
                foreach (var item in items)
                {
                    cart.items.Add(item, basket.items[item.Id]);
                }
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
            var reply = _client.GetItems(new GetItemsRequest { ItemIds = { Int64.Parse(inc) } });
            Item item = JsonSerializer.Deserialize<List<Item>>(reply.Items)![0];
            Basket basket;
            if (value == null)
                basket = new Basket();
            else
                basket = JsonSerializer.Deserialize<Basket>(value);
            if (basket.items[Int64.Parse(inc)] < item.Amount)
                basket.items[Int64.Parse(inc)] += 1;

            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize<Basket>(basket));
            return RedirectToAction("Cart");
        }

        [HttpGet]
        public IActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Order(Order order)
        {
            string value;
            HttpContext.Request.Cookies.TryGetValue("Cart", out value);
            if (value == null)
            {
                return RedirectToAction("Cart");
            }

            var reply = await _client.CreateOrderAsync(new CreateOrderRequest
                { Address = order.Address, Email = order.Email, Basket = value });
            Basket basket = JsonSerializer.Deserialize<Basket>(value)!;
            if (!reply.Success)
            {
                Item item = JsonSerializer.Deserialize<Item>(reply.ProblemItem)!;
                TempData["WrongAmountOfItem"] =
                    $"В наличии нет требуемого количества товара \"{item.Item_Code.Title} {item.Manufacturer.Title} {item.Title}\"." +
                    " Его количество в Вашей корзине задано максимально возможным. Приносим свои извинения!";
                basket.items[item.Id] = (int)item.Amount;
                HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize<Basket>(basket));
                    
                return RedirectToAction("Cart");
            }
            
            HttpContext.Response.Cookies.Delete("Cart");
            TempData["Success"] =
                "Новый заказ успешно создан. После обработки заказа, подтверждение и дальнейшие действия будут высланы на Ваш почтовый ящик. Спасибо!";
            return RedirectToAction("Table");
        }
    }
}