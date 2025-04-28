using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class KhachHang
    {
        [Key]
        [Required]
        public int MaKhachHang { get; set; }

        [Required]
        public string HoTen { get; set; } 

        [Phone]
        [Required]
        public string SoDienThoai { get; set; } 

        [EmailAddress]
        [Required]
        public string? Email { get; set; }
    }
}