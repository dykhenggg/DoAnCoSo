using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Ban
    {
        [Key]
        public int MaBan { get; set; }

        [Required]
        [StringLength(50)]
        public string TenBan { get; set; } = string.Empty;

        [Required]
        public int SucChua { get; set; }

        [Required]
        public bool TrangThai { get; set; }

        // Navigation property
        public virtual ICollection<DatBan> DatBan { get; set; } = new List<DatBan>();

        // Thêm trường theo dõi số lượng chỗ đã đặt
        public int SoLuongDatCho { get; set; } = 0;
    }
}