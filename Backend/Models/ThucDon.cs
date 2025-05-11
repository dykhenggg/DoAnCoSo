using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models
{
    // Trạng thái của món ăn: Còn hàng hoặc Hết hàng
    public enum TrangThaiMonAn
    {
        Available = 1,    // Còn hàng
        OutOfStock = 0    // Hết hàng
    }

    // Lớp ThucDon đại diện cho một món ăn trong menu
    public class ThucDon : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaMon { get; set; }      // Mã định danh của món ăn

        [Required(ErrorMessage = "Tên món không được để trống")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên món phải từ 3-100 ký tự")]
        public string TenMon { get; set; } = string.Empty;  // Tên của món ăn

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0, 10000000, ErrorMessage = "Giá phải từ 0đ đến 10.000.000đ")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }     // Giá của món ăn

        [Required]
        [StringLength(50)]
        public string LoaiMon { get; set; } = string.Empty;  // Phân loại món ăn (ví dụ: món chính, món phụ, tráng miệng)

        [Required]
        public string HinhAnh { get; set; } = string.Empty;  // Đường dẫn hình ảnh của món ăn

        public TrangThaiMonAn TrangThai { get; set; } = TrangThaiMonAn.Available;  // Trạng thái món ăn

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }

        // Navigation properties
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();
    }
}
