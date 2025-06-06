using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class DatBanChiTiet
    {
        [Key]
        public int MaChiTiet { get; set; }

        [Required]
        public int MaDatBan { get; set; }

        [Required]
        public int MaMon { get; set; }

        [Required]
        [Range(1, 100)]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ThanhTien { get; set; }

        public string? GhiChu { get; set; }

        // Navigation properties
        [ForeignKey("MaDatBan")]
        public virtual DatBan DatBan { get; set; } = null!;

        [ForeignKey("MaMon")]
        public virtual MonAn MonAn { get; set; } = null!;
    }
} 