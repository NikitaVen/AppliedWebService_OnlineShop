using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext() { }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Item> Items { get; set; }
    }
}
