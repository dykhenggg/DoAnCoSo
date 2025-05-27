using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class NhaCungCap
    {
        [Key]
        public int MaNCC { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNCC { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string SDT { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string DiaChi { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string TrangThai { get; set; } = "Active";

        // Navigation property
        public virtual ICollection<Kho> NguyenLieu { get; set; } = new List<Kho>();
    }
}