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

        // Thêm danh sách món ăn
        public List<DatBanMonAnDTO>? MonAn { get; set; }
    }

    public class DatBanMonAnDTO
    {
        [Required]
        public int MaMon { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SoLuong { get; set; }

        public string? GhiChu { get; set; }
    }
}
