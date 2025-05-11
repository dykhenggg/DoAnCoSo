using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string SoDienThoai { get; set; } = string.Empty;

        public virtual ICollection<DatBan> DatBan { get; set; } = new List<DatBan>();
        public virtual ICollection<DonHang> DonHang { get; set; } = new List<DonHang>();
    }
}