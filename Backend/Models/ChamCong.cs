using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Backend.Models
{
    public class ChamCong
    {
        [Key]
        public int MaChamCong { get; set; }

        [Required]
        [ForeignKey("NhanVien")]
        public int MaNhanVien { get; set; }

        public virtual NhanVien? NhanVien { get; set; }

        [Required]
        public DateTime NgayChamCong { get; set; }

        public TimeSpan GioVao { get; set; }

        public TimeSpan GioRa { get; set; }

        [Required]
        public string TrangThai { get; set; } = "Đang làm việc";
    }
}