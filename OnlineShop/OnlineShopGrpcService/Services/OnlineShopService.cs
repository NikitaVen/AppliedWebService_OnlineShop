using System.Text.Json;
using Google.Protobuf.Collections;
using OnlineShopGrpcService.Models;

namespace OnlineShopGrpcService.Services;

using Grpc.Core;
using OnlineShopGrpcService;

public class OnlineShopService : OnlineShop.OnlineShopBase
{
    private readonly ShopContext _context = new ShopContext();
    public OnlineShopService(ShopContext options)
    {
        _context = options;
    }

    public override async Task<GetItemsResponse> GetItems(GetItemsRequest request, ServerCallContext context)
    {
        if (request.ItemIds == null)
        {
            return await Task.FromResult(new GetItemsResponse
            {
                Items =  JsonSerializer.Serialize(_context.Items.ToList())
            });
        }

        List<Item> items = new List<Item>();
        for (int i = 0; i < request.ItemIds.Count; ++i)
        {
            items.Add(_context.Items.First(item=>item.Id == request.ItemIds[i]));
        }

        return await Task.FromResult(new GetItemsResponse
        {
            Items =  JsonSerializer.Serialize(items)
        });
    }

    public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        Order order = new Order();
        order.Address = request.Address;
        order.Email = request.Email;
        order.OrderItems = new List<OrderItem>();
        Basket basket = JsonSerializer.Deserialize<Basket>(request.Basket)!;
        decimal totalPrice = 0;
        foreach (KeyValuePair<long, int> entry in basket.items)
        {
            OrderItem oi = new OrderItem();
            oi.Item = _context.Items.First(i => i.Id == entry.Key);
            if (oi.Item.Amount < entry.Value)
            {
                return await Task.FromResult(new CreateOrderResponse
                {
                    Success = false,
                    ProblemItem = JsonSerializer.Serialize(oi.Item),
                    MaxAmount = oi.Item.Amount
                });
            }

            oi.Order = order;
            oi.Amount = entry.Value;
            totalPrice += oi.Item.Price * oi.Amount;
            order.OrderItems.Add(oi);
        }

        order.Order_date = DateTime.Now;
        order.TotalPrice = totalPrice;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return await Task.FromResult(new CreateOrderResponse
        {
            Success = true
        });
    }
}