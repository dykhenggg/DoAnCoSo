using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class LichLamViec
    {
        [Key]
        public int MaLichLamViec { get; set; }

        [Required]
        public int MaNhanVien { get; set; }

        [Required]
        public int MaCa { get; set; }

        [Required]
        public DateTime NgayLamViec { get; set; }

        public string GhiChu { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; }

        [ForeignKey("MaCa")]
        public virtual CaLamViec CaLamViec { get; set; }
    }
}