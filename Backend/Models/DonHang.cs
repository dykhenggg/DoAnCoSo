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
        public virtual KhachHang? KhachHang { get; set; }

        [Required]
        public DateTime NgayDat { get; set; }

        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; } = "ChoXacNhan"; // ChoXacNhan, DangChuanBi, HoanThanh, DaThanhToan, DaHuy

        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTien { get; set; }

        // Navigation property to HoaDon
        public virtual HoaDon? HoaDon { get; set; }

        public DonHang()
        {
            ChiTietDonHang = new HashSet<ChiTietDonHang>();
        }
    }
}