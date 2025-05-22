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
        public DbSet<ThucDon> ThucDon { get; set; }
        public DbSet<Ban> Ban { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<VaiTro> Role { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<KhuyenMai> KhuyenMai { get; set; }
        public DbSet<KhuyenMai_DonHang> KhuyenMai_DonHang { get; set; }
        public DbSet<CaLamViec> CaLamViec { get; set; }
        public DbSet<LichLamViec> LichLamViec { get; set; }
        public DbSet<ChamCong> ChamCong { get; set; }
        public DbSet<Kho> Kho { get; set; }
        public DbSet<GiaoDichKho> GiaoDichKho { get; set; }
        public DbSet<DatBan> DatBan { get; set; }
        public DbSet<BoPhan> BoPhan { get; set; }
        public DbSet<LoaiMon> LoaiMon { get; set; }

        // Tùy chỉnh nếu cần (mapping, relationship...)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ThucDon configurations
            modelBuilder.Entity<ThucDon>()
                .Property(t => t.Gia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ThucDon>()
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

            // Relationship configurations
            modelBuilder.Entity<KhuyenMai_DonHang>()
                .HasKey(k => new { k.MaDonHang, k.MaKhuyenMai });

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

            modelBuilder.Entity<CaLamViec>()
                .HasOne(c => c.NhanVien)
                .WithMany(n => n.CaLamViec)
                .HasForeignKey(c => c.MaNhanVien)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichLamViec>()
                .HasOne(l => l.NhanVien)
                .WithMany()
                .HasForeignKey(l => l.MaNhanVien)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChamCong>()
                .HasOne(c => c.NhanVien)
                .WithMany(n => n.ChamCong)
                .HasForeignKey(c => c.MaNhanVien)
                .OnDelete(DeleteBehavior.Restrict);

            // Value conversions for enums
            modelBuilder.Entity<DonHang>()
                .Property(d => d.TrangThai)
                .HasConversion<string>();

            modelBuilder.Entity<ThucDon>()
                .Property(t => t.TrangThai)
                .HasConversion<string>();

            modelBuilder.Entity<LichLamViec>()
                .HasIndex(l => new { l.MaNhanVien, l.NgayLamViec, l.MaCa })
                .IsUnique();

            // Seed BoPhan first
            modelBuilder.Entity<BoPhan>().HasData(
                new BoPhan
                {
                    MaBoPhan = 1,
                    TenBoPhan = "Ban Quản Lý"
                }
            );

            // Seed roles
            modelBuilder.Entity<VaiTro>().HasData(
                new VaiTro 
                { 
                    MaVaiTro = 1, 
                    TenVaiTro = UserRoles.QuanLy, 
                    MoTa = "Quản lý hệ thống" 
                },
                new VaiTro 
                { 
                    MaVaiTro = 2, 
                    TenVaiTro = UserRoles.NhanVien, 
                    MoTa = "Nhân viên" 
                },
                new VaiTro 
                { 
                    MaVaiTro = 3, 
                    TenVaiTro = UserRoles.KhachHang, 
                    MoTa = "Khách hàng" 
                }
            );

            // Update admin user seed with static hashed password
            modelBuilder.Entity<NhanVien>().HasData(
                new NhanVien
                {
                    MaNV = 1,
                    HoTen = "Admin",
                    Email = "admin@gmail.com",
                    // Pre-hashed value of "admin123"
                    MatKhau = "$2a$11$AHzAlAtAtx.m0g8q0x5nj.LB42KsSQx3hlzv7yE3UQGnn6oNhxIqm",
                    ChucVu = UserRoles.QuanLy,
                    SDT = "0123456789",
                    TrangThai = "Đang làm việc",
                    MaBoPhan = 1, // Link to Ban Quản Lý
                    DiaChi = "Admin Address"
                }
            );

            // Add BoPhan-NhanVien relationship configuration
            modelBuilder.Entity<NhanVien>()
                .HasOne(n => n.BoPhan)
                .WithMany(b => b.NhanVien)
                .HasForeignKey(n => n.MaBoPhan)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
