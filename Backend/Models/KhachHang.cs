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
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Phone]
        [Required]
        public string SoDienThoai { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public virtual ICollection<DonHang> DonHang { get; set; } = new List<DonHang>();
    }
}