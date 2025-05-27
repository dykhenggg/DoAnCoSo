using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class ThanhToanDTO
    {
        public int MaNhanVien { get; set; }
        public string PhuongThucThanhToan { get; set; } = "TienMat";
        public decimal GiamGia { get; set; }
        public string? GhiChu { get; set; }
    }
}