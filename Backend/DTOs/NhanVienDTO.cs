using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public enum UserRoles
    {
        QuanLy,
        NhanVien
    }

    public class NhanVienDTO
    {
        public string hoTen { get; set; }
        public string email { get; set; }
        public string? diaChi { get; set; }
        public string chucVu { get; set; }
        public int maBoPhan { get; set; }
        public string? matKhau { get; set; }
    }
}