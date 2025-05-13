using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Kho
    {
        [Key]
        public int MaNguyenLieu { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNguyenLieu { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DanhMuc { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DonVi { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SoLuongHienTai { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SoLuongToiThieu { get; set; }

        [Required]
        public DateTime NgayNhap { get; set; }

        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; } = "Active";
    }
}