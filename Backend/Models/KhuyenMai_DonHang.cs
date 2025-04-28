using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class KhuyenMai_DonHang
    {
        [ForeignKey("DonHang")]
        public int MaDonHang { get; set; }
        public DonHang DonHang { get; set; }

        [ForeignKey("KhuyenMai")]
        public int MaKhuyenMai { get; set; }
        public KhuyenMai KhuyenMai { get; set; }

        [Required]
        public decimal SoTienGiam { get; set; }
    }
}
