using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class VaiTro
    {
        [Key]
        [Required]
        public int MaVaiTro { get; set; }

        [Required]
        public string TenVaiTro { get; set; }
    }
}