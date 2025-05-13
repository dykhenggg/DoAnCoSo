using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class GiaoDichKhoDTO
    {
        [Required]
        public int MaNguyenLieu { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SoLuong { get; set; }

        public string? GhiChu { get; set; }
    }
}
