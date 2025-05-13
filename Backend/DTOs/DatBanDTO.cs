using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class DatBanDTO
    {
        [Required]
        public int MaBan { get; set; }

        [Required]
        public int MaKH { get; set; }

        [Required]
        public DateTime ThoiGianBatDau { get; set; }

        [Required]
        public DateTime ThoiGianKetThuc { get; set; }

        [Required]
        [Range(1, 20)]
        public int SoNguoi { get; set; }

        public string? GhiChu { get; set; }
    }
}
