using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class GiaoDichKhoDTO
    {
        [Required]
        public int MaNguyenLieu { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public decimal SoLuong { get; set; }

        [Required]
        public string LoaiGiaoDich { get; set; } = "nhap"; // "nhap" hoặc "xuat"

        public string? GhiChu { get; set; }
    }
}
