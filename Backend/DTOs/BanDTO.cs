using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class BanDTO
    {
        [Required]
        [StringLength(50)]
        public string TenBan { get; set; } = string.Empty;

        [Required]
        [Range(1, 50)]
        public int SucChua { get; set; }
    }
}
