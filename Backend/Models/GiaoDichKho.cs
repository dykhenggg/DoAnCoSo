using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class GiaoDichKho
    {
        [Key]
        public int MaGiaoDich { get; set; }

        [ForeignKey("Kho")]
        public int MaNguyenLieu { get; set; }

        [ForeignKey("DonHang")]
        public int? MaDonHang { get; set; } 

        [Required]
        [StringLength(20)]
        public string Loai { get; set; } // "NhapKho" | "XuatKho" | "HuyHang"...

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SoLuong { get; set; }

        [Required]
        public DateTime NgayGio { get; set; }

        [StringLength(255)]
        public string LyDo { get; set; }

        // Navigation properties
        public Kho Kho { get; set; }
        public DonHang DonHang { get; set; }
    }
}
