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
        public string TenLoai { get; set; }

        public string HinhAnh { get; set; }

        // Navigation property
        public virtual ICollection<ThucDon> ThucDons { get; set; } = new List<ThucDon>();
    }
}