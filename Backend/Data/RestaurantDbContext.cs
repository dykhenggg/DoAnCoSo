using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Models.Enums;

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
        public DbSet<MonAn> MonAn { get; set; }
        public DbSet<Ban> Ban { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<VaiTro> Role { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<KhuyenMai> KhuyenMai { get; set; }
        public DbSet<KhuyenMai_MonAn> KhuyenMai_MonAn { get; set; }
        public DbSet<LichSuKhuyenMai> LichSuKhuyenMai { get; set; }
        public DbSet<CaLamViec> CaLamViec { get; set; }
        public DbSet<LichLamViec> LichLamViec { get; set; }
        public DbSet<ChamCong> ChamCong { get; set; }
        public DbSet<Kho> Kho { get; set; }
        public DbSet<GiaoDichKho> GiaoDichKho { get; set; }
        public DbSet<DatBan> DatBan { get; set; }
        public DbSet<BoPhan> BoPhan { get; set; }
        public DbSet<LoaiMon> LoaiMon { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }
        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<NguyenLieu> NguyenLieu { get; set; } 

        // Override OnModelCreating để cấu hình các quan hệ và khóa chính
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // NhanVien configurations
            modelBuilder.Entity<NhanVien>()
                .HasOne(n => n.BoPhan)
                .WithMany(b => b.NhanVien)
                .HasForeignKey(n => n.MaBoPhan)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NhanVien>()
                .HasIndex(n => n.Email)
                .IsUnique();

            // Thêm dữ liệu mặc định cho vai trò
            modelBuilder.Entity<VaiTro>().HasData(
                new VaiTro
                {
                    MaVaiTro = 1,
                    TenVaiTro = "Quản lý",
                    MoTa = "Quản lý hệ thống với toàn quyền truy cập"
                },
                new VaiTro
                {
                    MaVaiTro = 2,
                    TenVaiTro = "Nhân viên",
                    MoTa = "Nhân viên với quyền truy cập hạn chế"
                }
            );

            // MonAn configurations
            modelBuilder.Entity<MonAn>()
                .Property(t => t.Gia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<MonAn>()
                .HasIndex(t => t.TenMon);

            // DonHang configurations
            modelBuilder.Entity<DonHang>()
                .Property(d => d.TongTien)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DonHang>()
                .HasIndex(d => d.NgayDat);

            // KhuyenMai configurations
            modelBuilder.Entity<KhuyenMai>()
                .HasIndex(k => new { k.NgayBatDau, k.NgayKetThuc });

            // Relationship configurations
            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(c => c.DonHang)
                .WithMany(d => d.ChiTietDonHang)
                .HasForeignKey(c => c.MaDonHang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DatBan>()
                .HasOne(d => d.Ban)
                .WithMany(b => b.DatBan)
                .HasForeignKey(d => d.MaBan)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ cho KhuyenMai_MonAn
            modelBuilder.Entity<KhuyenMai_MonAn>()
                .HasKey(k => new { k.MaKhuyenMai, k.MaMon });

            modelBuilder.Entity<KhuyenMai_MonAn>()
                .HasOne(k => k.KhuyenMai)
                .WithMany(k => k.KhuyenMai_MonAn)
                .HasForeignKey(k => k.MaKhuyenMai)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KhuyenMai_MonAn>()
                .HasOne(k => k.MonAn)
                .WithMany()
                .HasForeignKey(k => k.MaMon)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ cho LichSuKhuyenMai
            modelBuilder.Entity<LichSuKhuyenMai>()
                .HasOne(l => l.KhuyenMai)
                .WithMany(k => k.LichSuKhuyenMai)
                .HasForeignKey(l => l.MaKhuyenMai)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LichSuKhuyenMai>()
                .HasOne(l => l.DonHang)
                .WithMany()
                .HasForeignKey(l => l.MaDonHang)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

