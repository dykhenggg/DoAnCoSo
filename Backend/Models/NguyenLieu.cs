using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Base;

namespace Backend.Models{
    public class NguyenLieu
    {
    [Key]
    public int MaNguyenLieu { get; set; }

    [Required]
    public int MaMon { get; set; }

    [Required]
    public int MaNL { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal SoLuong { get; set; }

    [Required]
    [StringLength(20)]
    public string DonVi { get; set; } = string.Empty;

    [ForeignKey("MaMon")]
    public virtual MonAn MonAn { get; set; } = null!;

    [ForeignKey("MaNL")]
    public virtual Kho KhoNguyenLieu { get; set; } = null!;
    }
}
