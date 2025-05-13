using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LichLamViecDTO
    {
        [Required]
        public int MaNV { get; set; }

        [Required]
        public int MaCa { get; set; }

        [Required]
        public DateTime NgayLamViec { get; set; }

        public string? GhiChu { get; set; }
    }
}
