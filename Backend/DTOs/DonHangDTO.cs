using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class DonHangDTO
    {
        [Required]
        public int MaKhachHang { get; set; }
        public List<ChiTietDonHangDTO> ChiTietList { get; set; } = new();
        public string? GhiChu { get; set; }
    }

    public class ChiTietDonHangDTO
    {
        [Required]
        public int MaMon { get; set; }

        [Required]
        [Range(1, 100)]
        public int SoLuong { get; set; }
    }
}
