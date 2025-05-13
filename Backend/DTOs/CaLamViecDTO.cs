using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class CaLamViecDTO
    {
        [Required]
        public int MaNhanVien { get; set; }

        [Required]
        public TimeSpan GioBatDau { get; set; }

        [Required]
        public TimeSpan GioKetThuc { get; set; }
    }
}
