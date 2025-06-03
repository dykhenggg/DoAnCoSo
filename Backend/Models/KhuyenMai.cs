using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class KhuyenMai
    {
        [Key]
        public int MaKhuyenMai { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhuyenMai { get; set; } = string.Empty;

        [StringLength(500)]
        public string MoTa { get; set; } = string.Empty;

        [Required]
        public decimal PhanTramGiam { get; set; }

        [Required]
        public DateTime NgayBatDau { get; set; }

        [Required]
        public DateTime NgayKetThuc { get; set; }

        [Required]
        public string DieuKien { get; set; } = string.Empty;

        [Required]
        public bool TrangThai { get; set; } = true;

        // Quan hệ với món ăn - không yêu cầu khi tạo mới
        [NotMapped]
        public virtual ICollection<KhuyenMai_MonAn> KhuyenMai_MonAn { get; set; } = new List<KhuyenMai_MonAn>();
        
        // Quan hệ với lịch sử khuyến mãi - không yêu cầu khi tạo mới
        [NotMapped]
        public virtual ICollection<LichSuKhuyenMai> LichSuKhuyenMai { get; set; } = new List<LichSuKhuyenMai>();
    }

    // Model cho bảng trung gian
    public class KhuyenMai_MonAn
    {
        public int MaKhuyenMai { get; set; }
        public int MaMon { get; set; }

        public virtual KhuyenMai KhuyenMai { get; set; }
        public virtual MonAn MonAn { get; set; }
    }
}