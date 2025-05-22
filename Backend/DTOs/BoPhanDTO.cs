namespace Backend.DTOs
{
    public class BoPhanDTO
    {
        public int MaBoPhan { get; set; }
        public string TenBoPhan { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public int SoLuongNhanVien { get; set; }
    }
}
