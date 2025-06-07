using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class KiemKeKho
    {
        [Key]
        public int MaKiemKe { get; set; }

        [Required]
        public DateTime NgayKiemKe { get; set; }

        [Required]
        [StringLength(100)]
        public string NguoiKiemKe { get; set; } = string.Empty;

        public string GhiChu { get; set; } = string.Empty;

        public virtual ICollection<ChiTietKiemKe> ChiTietKiemKe { get; set; } = new List<ChiTietKiemKe>();
    }

    public class ChiTietKiemKe
    {
        [Key]
        public int MaChiTietKiemKe { get; set; }

        [Required]
        public int MaKiemKe { get; set; }

        [Required]
        public int MaNguyenLieu { get; set; }

        [Required]
        public decimal SoLuongThucTe { get; set; }

        public decimal ChenhLech { get; set; }

        public string GhiChu { get; set; } = string.Empty;

        [ForeignKey("MaKiemKe")]
        public virtual KiemKeKho KiemKeKho { get; set; } = null!;

        [ForeignKey("MaNguyenLieu")]
        public virtual Kho NguyenLieu { get; set; } = null!;
    }
} 