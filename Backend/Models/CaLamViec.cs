using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class CaLamViec
    {
        [Key]
        [Required]
        public int MaCa { get; set; }

        [Required]
        public TimeSpan GioBatDau { get; set; }

        [Required]
        public TimeSpan GioKetThuc { get; set; }

        // Liên kết với nhân viên
        [Required]
        public int MaNhanVien { get; set; } 

        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; } = null!; // Navigation Property
    }
}
