using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public enum TrangThaiMonAn
    {
        Available,
        OutOfStock
    }

    public class ThucDon
    {
        [Key]
        [Required]
        public int MaMon { get; set; }

        [Required]
        [StringLength(100)] // Giới hạn độ dài tên món ăn
        public string TenMon { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không thể âm.")] // Kiểm tra giá không âm
        public decimal Gia { get; set; }

        [Required]
        public string LoaiMon { get; set; } // Loại món có thể là không null

        [Required]
        public string HinhAnh { get; set; } // Hình ảnh không cần nullable nếu bắt buộc

        [Required]
        public TrangThaiMonAn TrangThai { get; set; } // Sử dụng enum để quản lý trạng thái
    }
}
