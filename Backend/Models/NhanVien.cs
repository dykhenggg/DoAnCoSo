using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table(name : "NhanVien")]
    public class NhanVien
    {
        [Key]
        public int MaNhanVien { get; set; }

        [ForeignKey("VaiTro")]
        public int MaVaiTro { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten { get; set; }

        [Required]
        [StringLength(50)]
        public string ChucVu { get; set; }

        [Phone]
        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        // Navigation property
        public VaiTro VaiTro { get; set; }
    }
}
