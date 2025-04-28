using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class CT_DonHang
    {
        [ForeignKey(nameof(DonHang))]
        public int MaDonHang { get; set; }

        [ForeignKey(nameof(ThucDon))]
        public int MaMon { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public decimal GiaTaiThoiDiem { get; set; }

        // Navigation properties
        public DonHang DonHang { get; set; }
        public ThucDon ThucDon { get; set; }
    }
}
