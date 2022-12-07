using Microsoft.EntityFrameworkCore;

namespace OnlineShopGrpcService.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext()
        {
        }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasOne(a => a.Manufacturer)
                .WithMany()
                .HasForeignKey(i => i.ID_Manufacturer)
                .IsRequired();

            modelBuilder.Entity<Item>()
                .Navigation(i => i.Manufacturer)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .AutoInclude();

            modelBuilder.Entity<Item>()
                .HasOne(a => a.Item_Code)
                .WithMany()
                .HasForeignKey(i=>i.ItemCode)
                .IsRequired();

            modelBuilder.Entity<Item>()
                .Navigation(i => i.Item_Code)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .AutoInclude();
            
            modelBuilder.Entity<OrderItem>().HasKey(oi => new { oi.IdOrder, oi.IdItem });

            modelBuilder.Entity<OrderItem>()
                .HasOne<Item>(sc => sc.Item)
                .WithMany()
                .HasForeignKey(oi => oi.IdItem);
            
            modelBuilder.Entity<OrderItem>().Navigation(oi => oi.Item)
                .UsePropertyAccessMode(PropertyAccessMode.Property).AutoInclude();
            modelBuilder.Entity<OrderItem>().Navigation(oi => oi.Order)
                .UsePropertyAccessMode(PropertyAccessMode.Property).AutoInclude();
            
            modelBuilder.Entity<Order>().Navigation(o => o.OrderItems)
                .UsePropertyAccessMode(PropertyAccessMode.Property).AutoInclude();

            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.IdOrder);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Item_code> ItemCodes { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}