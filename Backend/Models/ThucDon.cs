using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models
{
    public class ThucDon : BaseEntity
    {
        [Key]
        [Required]
        public int MaMon { get; set; }

        [Required]
        [StringLength(100)] // Giới hạn độ dài tên món ăn
        public string? TenMon { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không thể âm.")] // Kiểm tra giá không âm
        public decimal Gia { get; set; }

        [Required]
        public string? LoaiMon { get; set; } // Loại món có thể là không null

        [Required]
        public string? HinhAnh { get; set; } // Hình ảnh không cần nullable nếu bắt buộc

        [Required]
        public TrangThaiMonAn TrangThai { get; set; } // Sử dụng enum để quản lý trạng thái

        public int SoLuongTon { get; set; }

        [NotMapped]
        public bool CoTheBan => TrangThai == TrangThaiMonAn.Available &&
                               SoLuongTon > 0;

        [NotMapped]
        public decimal GiaSauGiam => Gia * (1 - (KhuyenMai?.PhanTramGiam ?? 0) / 100);

        public virtual KhuyenMai? KhuyenMai { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();
    }
}