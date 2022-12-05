using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    [Table("Item")]
    public class Item
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Column("Item_Code")]
        public long ItemСode { get; set; }

       // public virtual Item_code Item_Code { get; set; }

        [Required]
        public long ID_Manufacturer { get; set; }

       // public Manufacturer Manufacturer { get; set; }

        public byte[] Image { get; set; }
        public string Description { get; set; }

        [Required]
        public long Amount { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
