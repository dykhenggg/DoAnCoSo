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

            // NhanVien configurations
            modelBuilder.Entity<NhanVien>()
                .HasIndex(n => n.Email)
                .IsUnique();

            modelBuilder.Entity<NhanVien>()
                .HasIndex(n => n.SDT)
                .IsUnique();

            // // User configurations
            // modelBuilder.Entity<User>()
            //     .HasIndex(u => u.Email)
            //     .IsUnique();

            // modelBuilder.Entity<User>()
            //     .HasIndex(u => u.SDT)
            //     .IsUnique();

            // modelBuilder.Entity<User>()
            //     .HasOne(u => u.VaiTro)
            //     .WithMany(r => r.Users)
            //     .HasForeignKey(u => u.MaVaiTro)
            //     .OnDelete(DeleteBehavior.Restrict);

            // Relationship configurations
            // modelBuilder.Entity<KhuyenMai_DonHang>()
            //     .HasKey(k => new { k.MaDonHang, k.MaKhuyenMai });

            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(c => c.DonHang)
                .WithMany(d => d.ChiTietDonHang)
                .HasForeignKey(c => c.MaDonHang)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DatBan>()
                .HasOne(d => d.Ban)
                .WithMany(b => b.DatBan)
                .HasForeignKey(d => d.MaBan)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
// Xóa các dòng sau:
// public DbSet<User> Users { get; set; }
// public DbSet<KhuyenMai_DonHang> KhuyenMai_DonHangs { get; set; }

// Và xóa tất cả các đoạn code khởi tạo dữ liệu liên quan đến User trong OnModelCreating
