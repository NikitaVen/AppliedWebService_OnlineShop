using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopGrpcService.Models;
[Table("Ord_Item")]
public class OrderItem
{
    [Column("ID_Ord")]
    public long IdOrder { get; set; }
    public Order Order { get; set; }
    [Column("ID_Item")]
    public long IdItem { get; set; }
    public Item Item { get; set; }
    public long Amount { get; set; }
}