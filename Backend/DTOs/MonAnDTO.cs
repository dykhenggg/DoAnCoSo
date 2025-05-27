using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class MonAnDTO
    {
        [Required(ErrorMessage = "Tên món không được để trống")]
        [StringLength(100, ErrorMessage = "Tên món không được vượt quá 100 ký tự")]
        public string TenMon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không thể âm")]
        public decimal Gia { get; set; }

        [Required(ErrorMessage = "Mã loại món không được để trống")]
        public int MaLoai { get; set; }

        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Hình ảnh không được để trống")]
        public IFormFile HinhAnh { get; set; } = null!;

        public List<NguyenLieuDTO> NguyenLieu { get; set; } = new();
    }

    public class NguyenLieuDTO
    {
        [Required(ErrorMessage = "Mã nguyên liệu không được để trống")]
        public int MaNL { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng không thể âm")]
        public decimal SoLuong { get; set; }

        [Required(ErrorMessage = "Đơn vị không được để trống")]
        [StringLength(20, ErrorMessage = "Đơn vị không được vượt quá 20 ký tự")]
        public string DonVi { get; set; } = string.Empty;
    }
}

    
