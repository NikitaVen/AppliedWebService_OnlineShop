namespace OnlineShop.Models
{
    public class Basket
    {
        public Dictionary<Item, int> items { get; set; }

        public Basket()
        {
            items = new Dictionary<Item, int>();
        }
    }
}
