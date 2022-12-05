using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    //[Table("Item")]
    public class Item
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        //[Required]
        //[Column("Item_code")]
        //public Item_code ItemCode { get; set; }

        //[Required]
        //[Column("ID_Manufacturer")]
        //public Manufacturer Manufacturer { get; set; }

        public byte[] Image { get; set; }
        public string Description { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
