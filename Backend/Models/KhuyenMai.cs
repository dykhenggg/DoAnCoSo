using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class KhuyenMai
    {
        [Key]
        public int MaKM { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKM { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }

        [Required]
        public decimal PhanTramGiam { get; set; }

        [Required]
        public DateTime NgayBatDau { get; set; }

        [Required]
        public DateTime NgayKetThuc { get; set; }
    }
}