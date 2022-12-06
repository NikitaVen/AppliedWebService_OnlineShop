using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models;

[Table("Ord")]
public class Order
{
    public long Id { get; set; }
    public DateTime Order_date { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}