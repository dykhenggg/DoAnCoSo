using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Kho
    {
        [Key]
        public int MaNguyenLieu { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNguyenLieu { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SoLuongTonKho { get; set; }

        [Required]
        [StringLength(20)]
        public string DonViTinh { get; set; } = string.Empty;

        public virtual ICollection<GiaoDichKho> GiaoDichKhos { get; set; } = new List<GiaoDichKho>();
    }
}
