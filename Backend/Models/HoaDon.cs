using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class HoaDon
    {
        [Key]
        public int MaHoaDon { get; set; }

        [Required]
        public int MaDonHang { get; set; }

        [Required]
        public DateTime ThoiGianThanhToan { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTienHang { get; set; } // Tổng tiền các món

        [Column(TypeName = "decimal(18,2)")]
        public decimal GiamGia { get; set; } // Số tiền giảm giá (nếu có)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TongThanhToan { get; set; } // Số tiền cuối cùng phải thanh toán

        [Required]
        [StringLength(20)]
        public string PhuongThucThanhToan { get; set; } = "TienMat"; // TienMat, ChuyenKhoan, MoMo, ...

        [Required]
        public int MaNhanVien { get; set; } // Nhân viên thu ngân

        [StringLength(200)]
        public string? GhiChu { get; set; }

        // Navigation properties
        [ForeignKey("MaDonHang")]
        public virtual DonHang DonHang { get; set; } = null!;

        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; } = null!;
    }
}