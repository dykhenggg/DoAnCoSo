using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class KhuyenMai_DonHang
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("DonHang")]
        public int MaDonHang { get; set; }
        public virtual DonHang DonHang { get; set; } = null!;

        [Key]
        [Column(Order = 2)]
        [ForeignKey("KhuyenMai")]
        public int MaKhuyenMai { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; } = null!;

        [Required]
        public decimal SoTienGiam { get; set; }
    }
}
