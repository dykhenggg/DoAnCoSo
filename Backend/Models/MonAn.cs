using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models
{
    public class MonAn : BaseEntity
    {
        [Key]
        [Required]
        public int MaMon { get; set; }

        [Required]
        [StringLength(100)]
        public string TenMon { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không thể âm.")]
        public decimal Gia { get; set; }

        [Required]
        [ForeignKey("LoaiMon")]
        public int MaLoai { get; set; }
        public required LoaiMon LoaiMon { get; set; }

        [Required]
        public string HinhAnh { get; set; } = string.Empty;

        [NotMapped]
        public decimal GiaSauGiam => Gia * (1 - (KhuyenMai_MonAn?.FirstOrDefault()?.KhuyenMai?.PhanTramGiam ?? 0) / 100);

        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();
    
        // Thêm quan hệ với NguyenLieu
        public virtual ICollection<NguyenLieu> NguyenLieu { get; set; } = new List<NguyenLieu>();

        // Quan hệ nhiều-nhiều với KhuyenMai
        public virtual ICollection<KhuyenMai_MonAn> KhuyenMai_MonAn { get; set; } = new List<KhuyenMai_MonAn>();
    }
}
