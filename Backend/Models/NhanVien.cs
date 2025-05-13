using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class NhanVien
    {
        [Key]
        public int MaNV { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string SDT { get; set; } = string.Empty;

        [StringLength(200)]
        public string DiaChi { get; set; } = string.Empty;

        [Required]
        public string ChucVu { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; } = "Đang làm việc";

        [Required]
        public string MatKhau { get; set; } = string.Empty;

        public int MaBoPhan { get; set; }
        public virtual BoPhan BoPhan { get; set; } = null!;

        // Collection navigation property
        public virtual ICollection<CaLamViec> CaLamViec { get; set; } = new List<CaLamViec>();
        public virtual ICollection<ChamCong> ChamCong { get; set; } = new List<ChamCong>();
    }
}