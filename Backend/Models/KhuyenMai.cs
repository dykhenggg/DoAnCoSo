using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class KhuyenMai
    {
        [Key]
        [Required]
        public int MaKhuyenMai { get; set; }

        [Required]
        public string Ten { get; set; }

        [Required]
        public string Loai { get; set; }

        [Required]
        public decimal GiaTriGiam { get; set; }
    }
}
