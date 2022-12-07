namespace OnlineShopGrpcService.Models
{
    public class Basket
    {
        public Dictionary<long, int> items { get; set; }

        public Basket()
        {
            items = new Dictionary<long, int>();
        }
    }
}
