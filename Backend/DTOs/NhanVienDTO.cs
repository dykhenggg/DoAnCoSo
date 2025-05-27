using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class NhanVienDTO
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(50, ErrorMessage = "Email không được vượt quá 50 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        public string SDT { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string DiaChi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        [StringLength(50, ErrorMessage = "Chức vụ không được vượt quá 50 ký tự")]
        public string ChucVu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6-100 ký tự")]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã bộ phận không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã bộ phận không hợp lệ")]
        public int MaBoPhan { get; set; }
    }
}