namespace OnlineShopGrpcService.Models
{
    public class Cart
    {
        public Dictionary<Item, int> items { get; set; }

        public Cart()
        {
            items = new Dictionary<Item, int>();
        }
    }
}
