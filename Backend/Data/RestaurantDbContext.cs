using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    // Kết nối với cơ sở dữ liệu
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        // DbSet cho từng bảng
        public DbSet<ThucDon> ThucDon { get; set; }
        public DbSet<Ban> Ban { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<VaiTro> VaiTro { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<CT_DonHang> CT_DonHang { get; set; }
        public DbSet<KhuyenMai> KhuyenMai { get; set; }
        public DbSet<KhuyenMai_DonHang> KhuyenMai_DonHang { get; set; }
        public DbSet<CaLamViec> CaLamViec { get; set; }
        public DbSet<LichLamViec> LichLamViec { get; set; }
        public DbSet<ChamCong> ChamCong { get; set; }
        public DbSet<Kho> Kho { get; set; }
        public DbSet<GiaoDichKho> GiaoDichKho { get; set; }
        public DbSet<DatBan> DatBan { get; set; }

        // Tùy chỉnh nếu cần (mapping, relationship...)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key cho CT_DonHang
            modelBuilder.Entity<CT_DonHang>()
                .HasKey(c => new { c.MaDonHang, c.MaMon });

            // Composite key cho KhuyenMaiDonHang
            modelBuilder.Entity<KhuyenMai_DonHang>()
                .HasKey(k => new { k.MaDonHang, k.MaKhuyenMai });
        }
    }
}
