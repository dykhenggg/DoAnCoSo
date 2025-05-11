using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) { }

        // Khai báo các DbSet cho từng model
        public DbSet<ThucDon> ThucDon { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<Ban> Ban { get; set; }
        public DbSet<DatBan> DatBan { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<CaLamViec> CaLamViec { get; set; }
        public DbSet<ChamCong> ChamCong { get; set; }
        public DbSet<KhuyenMai> KhuyenMai { get; set; }
        public DbSet<Kho> Kho { get; set; }
        public DbSet<GiaoDichKho> GiaoDichKho { get; set; }
        public DbSet<LichLamViec> LichLamViec { get; set; }
        public DbSet<KhuyenMai_DonHang> KhuyenMai_DonHang { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình relationships giữa các entity
            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(c => c.DonHang)
                .WithMany(d => d.ChiTietDonHang)
                .HasForeignKey(c => c.MaDonHang);

            modelBuilder.Entity<DatBan>()
                .HasOne(d => d.KhachHang)
                .WithMany()
                .HasForeignKey(d => d.MaKH);

            modelBuilder.Entity<KhuyenMai_DonHang>()
                .HasKey(k => new { k.MaDonHang, k.MaKhuyenMai });

            base.OnModelCreating(modelBuilder);
        }
    }
}
