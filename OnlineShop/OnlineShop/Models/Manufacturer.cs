using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnlineShop.Models
{
    [Table("Manufacturer")]
    public class Manufacturer
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
