using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class LichLamViec
    {
        [Key]
        public int MaLichLamViec { get; set; }

        [Required]
        [ForeignKey("NhanVien")]
        public int MaNV { get; set; }

        [Required]
        public DateTime NgayLam { get; set; }

        public string GhiChu { get; set; } = string.Empty;

        public virtual NhanVien NhanVien { get; set; } = null!;
        public virtual CaLamViec CaLamViec { get; set; } = null!;
    }
}