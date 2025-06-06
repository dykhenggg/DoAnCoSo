using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class KhoDTO
    {
        [Required]
        [StringLength(100)]
        public string TenNguyenLieu { get; set; } = string.Empty;

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
        public int MaNCC { get; set; }

        [Required]
        public string TrangThai { get; set; } = "Active";
    }
} 