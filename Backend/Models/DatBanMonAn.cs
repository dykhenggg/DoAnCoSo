using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Base;

namespace Backend.Models
{
    public class DatBanMonAn : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int MaDatBan { get; set; }

        [Required]
        public int MaMon { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        public string? GhiChu { get; set; }

        [ForeignKey("MaDatBan")]
        public virtual DatBan? DatBan { get; set; }

        [ForeignKey("MaMon")]
        public virtual MonAn? MonAn { get; set; }
    }
} 