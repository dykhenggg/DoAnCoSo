using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class ThucDonDTO
    {
        [Required]
        public string TenMon { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Gia { get; set; }

        [Required]
        public int MaLoai { get; set; }  // Thay đổi từ string LoaiMon sang int MaLoai

        public string? MoTa { get; set; }

        [Required]
        public IFormFile HinhAnh { get; set; } = null!;
    }
}
