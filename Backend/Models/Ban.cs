using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Ban
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaBan { get; set; }

        [Required]
        [StringLength(20)]
        public string TenBan { get; set; } = string.Empty;

        [Required]
        [Range(1, 20)]
        public int SoChoNgoi { get; set; }

        [Required]
        public bool TrangThai { get; set; } = true;

        public virtual ICollection<DatBan> DatBan { get; set; } = new List<DatBan>();
    }
}