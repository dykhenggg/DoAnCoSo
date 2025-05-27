using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTiet { get; set; }

        [Required]
        public int MaDonHang { get; set; }

        [Required]
        public int MaMon { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ThanhTien { get; set; }

        [ForeignKey("MaDonHang")]
        public virtual DonHang? DonHang { get; set; }

        [ForeignKey("MaMon")]
        public virtual MonAn? MonAn { get; set; }
    }
}