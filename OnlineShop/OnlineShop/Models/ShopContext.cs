using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models
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

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Item_code> ItemCodes { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
    }
}