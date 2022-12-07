using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace OnlineShop.Models;

[Table("Ord")]
public class Order
{
    public long Id { get; set; }
    public DateTime Order_date { get; set; }

    [EmailAddress(ErrorMessage = "Неверный формат Email")]
    [Required(ErrorMessage = "Поле Email обязательно")]
    public string Email { get; set; }

   [Required(ErrorMessage = "Поле Address обязательно")]
  //  [DataType(DataType.Text)]
    public string Address { get; set; }
    
    [Column("Total_Price")]
    public decimal TotalPrice { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}