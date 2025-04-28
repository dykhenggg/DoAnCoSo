using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class LichLamViec
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int MaNhanVien { get; set; }

        [Required]
        public int MaCa { get; set; }

        [Required]
        public DateTime NgayLamViec { get; set; }

        [ForeignKey("MaNhanVien")]
        public NhanVien NhanVien { get; set; }

        [ForeignKey("MaCa")]
        public CaLamViec CaLamViec { get; set; }
    }
}

