using System.ComponentModel.DataAnnotations;
using Backend.Models.Enums;

namespace Backend.DTOs
{
    public class NhanVienDTO
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string hoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string email { get; set; } = string.Empty;

        public string? diaChi { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        public string chucVu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã bộ phận không được để trống")]
        public int maBoPhan { get; set; }

        public string? matKhau { get; set; }
    }
}