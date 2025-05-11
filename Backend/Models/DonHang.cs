using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models
{
    public enum TrangThaiDonHang
    {
        DangChuanBi = 0,
        DangPhucVu = 1,
        DaThanhToan = 2,
        DaHuy = 3
    }

    public class DonHang : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDonHang { get; set; }

        [Required]
        public int MaKhachHang { get; set; }

        [Required]
        public DateTime NgayDat { get; set; } = DateTime.Now;

        public DateTime? ThoiGianThanhToan { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TongTien { get; set; }

        public TrangThaiDonHang TrangThai { get; set; } = TrangThaiDonHang.DangChuanBi;

        [Required]
        public int MaNV { get; set; }

        public string? GhiChu { get; set; }

        // Navigation properties
        [ForeignKey("MaKhachHang")]
        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();
        public virtual ICollection<KhuyenMai_DonHang> KhuyenMai_DonHang { get; set; } = new List<KhuyenMai_DonHang>();

        public DonHang()
        {
            ChiTietDonHang = new HashSet<ChiTietDonHang>();
        }
    }
}