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
        [StringLength(50)]
        public string TenVaiTro { get; set; } = string.Empty;

        [StringLength(200)]
        public string MoTa { get; set; } = string.Empty;
    }
}