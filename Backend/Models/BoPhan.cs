using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class BoPhan
    {
        [Key]
        public int MaBoPhan { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenBoPhan { get; set; } = string.Empty;

        public virtual ICollection<NhanVien> NhanVien { get; set; } = new List<NhanVien>();
    }
}