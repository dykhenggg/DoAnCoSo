using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LoginDTO
    {
        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        public string MatKhau { get; set; } = string.Empty;
    }

    public class TaiKhoanDTO
    {
        public int MaTaiKhoan { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SDT { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public DateTime NgayThamGia { get; set; }
        public string TenVaiTro { get; set; } = string.Empty;
    }

    public class RegisterTaiKhoanDTO
    {
        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(15)]
        public string SDT { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        public int MaVaiTro { get; set; }
    }
}
