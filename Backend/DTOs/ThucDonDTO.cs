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
        public string LoaiMon { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        [Required]
        public IFormFile HinhAnh { get; set; } = null!;
    }
}
