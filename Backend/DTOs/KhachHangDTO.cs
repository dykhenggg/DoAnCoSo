using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class KhachHangDTO
    {
        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string SoDienThoai { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }
    }
}
