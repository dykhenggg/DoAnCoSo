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

        public string GhiChu { get; set; }

        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("MaBan")]
        public virtual Ban Ban { get; set; }
    }
}