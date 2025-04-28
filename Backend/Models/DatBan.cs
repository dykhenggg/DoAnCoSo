using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class DatBan
    {
        [Key]
        public int MaDatBan { get; set; }

        [ForeignKey("KhachHang")]
        public int MaKhachHang { get; set; }

        [ForeignKey("Ban")]
        public int MaBan { get; set; }

        [Required]
        public DateTime ThoiGianDat { get; set; }

        [Required]
        public int SoNguoi { get; set; }

        [Required]
        [StringLength(30)]
        public string TrangThai { get; set; }

        // Navigation properties
        public KhachHang KhachHang { get; set; }
        public Ban Ban { get; set; }
    }
}
