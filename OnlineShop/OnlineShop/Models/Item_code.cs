using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
   // [Table("Item_code")]
    public class Item_code
    {
        [Key]
        public long Code { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
