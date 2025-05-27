using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Base;

namespace Backend.Models
{
    public class LoaiMon : BaseEntity
    {
        [Key]
        [Required]
        public int MaLoai { get; set; }

        [Required]
        [StringLength(50)]
        public required string TenLoai { get; set; }

        [Required]
        public required string HinhAnh { get; set; }

        // Navigation property
        public virtual ICollection<MonAn> MonAns { get; set; } = new List<MonAn>();
    }
}