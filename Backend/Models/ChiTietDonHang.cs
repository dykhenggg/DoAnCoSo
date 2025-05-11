using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTiet { get; set; }

        [Required]
        [ForeignKey("DonHang")]
        public int MaDonHang { get; set; }

        [Required]
        [ForeignKey("ThucDon")]
        public int MaMon { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SoLuong { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DonGia { get; set; }

        public string GhiChu { get; set; } = string.Empty;

        public virtual DonHang DonHang { get; set; } = null!;
        public virtual ThucDon ThucDon { get; set; } = null!;
    }
}