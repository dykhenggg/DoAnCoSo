using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class DatBan
    {
        [Key]
        public int MaDatBan { get; set; }

        [Required]
        public int MaKH { get; set; }

        [Required]
        public int MaBan { get; set; }

        [Required]
        public DateTime NgayDat { get; set; }

        [Required]
        public DateTime ThoiGianBatDau { get; set; }

        [Required]
        public DateTime ThoiGianKetThuc { get; set; }

        [Required]
        [Range(1, 20)]
        public int SoNguoi { get; set; }

        public string? GhiChu { get; set; }

        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; } = null!;

        [ForeignKey("MaBan")] 
        public virtual Ban Ban { get; set; } = null!;

        // Navigation property for menu items
        public virtual ICollection<DatBanMonAn> DatBanMonAn { get; set; } = new List<DatBanMonAn>();
    }
}