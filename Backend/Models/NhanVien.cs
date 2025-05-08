using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class NhanVien
    {
        [Key]
        public int MaNV { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string SDT { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [Required]
        public string ChucVu { get; set; }

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; }

        // Collection navigation property
        public virtual ICollection<CaLamViec> CaLamViec { get; set; }
        public virtual ICollection<ChamCong> ChamCong { get; set; }
    }
}