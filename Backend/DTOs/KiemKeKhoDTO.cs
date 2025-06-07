using System;
using System.Collections.Generic;

namespace Backend.DTOs
{
    public class KiemKeKhoDTO
    {
        public DateTime NgayKiemKe { get; set; }
        public string NguoiKiemKe { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public List<ChiTietKiemKeDTO> ChiTietKiemKe { get; set; } = new List<ChiTietKiemKeDTO>();
    }

    public class ChiTietKiemKeDTO
    {
        public int MaNguyenLieu { get; set; }
        public decimal SoLuongThucTe { get; set; }
        public decimal ChenhLech { get; set; }
        public string GhiChu { get; set; } = string.Empty;
    }

    public class KiemKeKhoResponseDTO
    {
        public int MaKiemKe { get; set; }
        public DateTime NgayKiemKe { get; set; }
        public string NguoiKiemKe { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public List<ChiTietKiemKeResponseDTO> ChiTietKiemKe { get; set; } = new List<ChiTietKiemKeResponseDTO>();
    }

    public class ChiTietKiemKeResponseDTO
    {
        public int MaChiTietKiemKe { get; set; }
        public int MaNguyenLieu { get; set; }
        public string TenNguyenLieu { get; set; } = string.Empty;
        public string DonVi { get; set; } = string.Empty;
        public decimal SoLuongHienTai { get; set; }
        public decimal SoLuongThucTe { get; set; }
        public decimal ChenhLech { get; set; }
        public string GhiChu { get; set; } = string.Empty;
    }
} 