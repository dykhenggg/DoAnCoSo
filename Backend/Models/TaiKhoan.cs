using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Base;

namespace Backend.Models
{
    public class TaiKhoan : BaseEntity
    {
        [Key]
        public int MaTaiKhoan { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(15)]
        public string SDT { get; set; } = string.Empty;

        [Required]
        public string MatKhauHash { get; set; } = string.Empty;

        [Required]
        [ForeignKey("VaiTro")]
        public int MaVaiTro { get; set; }
        public virtual VaiTro VaiTro { get; set; } = null!;

        [Required]
        [StringLength(20)]
        [RegularExpression("^(Active|Inactive|Locked)$")]
        public string TrangThai { get; set; } = "Active";

        [Required]
        public DateTime NgayThamGia { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }
        public int LoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }

        // Thêm mối quan hệ với NhanVien
        public int? MaNhanVien { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual NhanVien? NhanVien { get; set; }
    }
}