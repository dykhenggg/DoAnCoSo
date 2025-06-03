using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class LichSuKhuyenMai
    {
        [Key]
        public int MaLichSu { get; set; }

        public int MaKhuyenMai { get; set; }
        public int MaDonHang { get; set; }
        public decimal SoTienGiam { get; set; }
        public DateTime NgayApDung { get; set; }

        // Quan hệ với khuyến mãi
        [ForeignKey("MaKhuyenMai")]
        public virtual KhuyenMai KhuyenMai { get; set; }

        // Quan hệ với đơn hàng
        [ForeignKey("MaDonHang")]
        public virtual DonHang DonHang { get; set; }
    }
} 