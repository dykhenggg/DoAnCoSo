using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class DonHang
    {
        [Key]
        [Required]
        public int MaDonHang { get; set; }

        [ForeignKey("KhachHang")]
        public int KhachHangID { get; set; }
        public KhachHang KhachHang { get; set; }

        [ForeignKey("ThucDon")]
        public int ThucDonID { get; set; }
        public ThucDon ThucDon { get; set; }

        [Required]
        public DateTime NgayDat { get; set; }

        [Required]
        public string TrangThai { get; set; } // e.g., "Đang xử lý", "Đã hoàn thành", "Đã hủy"
    }
}