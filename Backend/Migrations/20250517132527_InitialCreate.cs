using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ban",
                columns: table => new
                {
                    MaBan = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenBan = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SucChua = table.Column<int>(type: "integer", nullable: false),
                    TrangThai = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ban", x => x.MaBan);
                });

            migrationBuilder.CreateTable(
                name: "BoPhan",
                columns: table => new
                {
                    MaBoPhan = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenBoPhan = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoPhan", x => x.MaBoPhan);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKhachHang = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HoTen = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.MaKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "Kho",
                columns: table => new
                {
                    MaNguyenLieu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenNguyenLieu = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DanhMuc = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DonVi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SoLuongHienTai = table.Column<decimal>(type: "numeric", nullable: false),
                    SoLuongToiThieu = table.Column<decimal>(type: "numeric", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TrangThai = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kho", x => x.MaNguyenLieu);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    MaKM = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenKM = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PhanTramGiam = table.Column<decimal>(type: "numeric", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.MaKM);
                });

            migrationBuilder.CreateTable(
                name: "LoaiMon",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenLoai = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HinhAnh = table.Column<string>(type: "text", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NguoiTao = table.Column<string>(type: "text", nullable: true),
                    NguoiCapNhat = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiMon", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenVaiTro = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HoTen = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SDT = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    DiaChi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ChucVu = table.Column<string>(type: "text", nullable: false),
                    TrangThai = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "text", nullable: false),
                    MaBoPhan = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.MaNV);
                    table.ForeignKey(
                        name: "FK_NhanVien_BoPhan_MaBoPhan",
                        column: x => x.MaBoPhan,
                        principalTable: "BoPhan",
                        principalColumn: "MaBoPhan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DatBan",
                columns: table => new
                {
                    MaDatBan = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaKH = table.Column<int>(type: "integer", nullable: false),
                    MaBan = table.Column<int>(type: "integer", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SoNguoi = table.Column<int>(type: "integer", nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatBan", x => x.MaDatBan);
                    table.ForeignKey(
                        name: "FK_DatBan_Ban_MaBan",
                        column: x => x.MaBan,
                        principalTable: "Ban",
                        principalColumn: "MaBan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatBan_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KhachHangID = table.Column<int>(type: "integer", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TrangThai = table.Column<string>(type: "text", nullable: false),
                    TongTien = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHang_KhachHang_KhachHangID",
                        column: x => x.KhachHangID,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThucDon",
                columns: table => new
                {
                    MaMon = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenMon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaLoai = table.Column<int>(type: "integer", nullable: false),
                    HinhAnh = table.Column<string>(type: "text", nullable: false),
                    TrangThai = table.Column<string>(type: "text", nullable: false),
                    SoLuongTon = table.Column<int>(type: "integer", nullable: false),
                    KhuyenMaiMaKM = table.Column<int>(type: "integer", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NguoiTao = table.Column<string>(type: "text", nullable: true),
                    NguoiCapNhat = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThucDon", x => x.MaMon);
                    table.ForeignKey(
                        name: "FK_ThucDon_KhuyenMai_KhuyenMaiMaKM",
                        column: x => x.KhuyenMaiMaKM,
                        principalTable: "KhuyenMai",
                        principalColumn: "MaKM");
                    table.ForeignKey(
                        name: "FK_ThucDon_LoaiMon_MaLoai",
                        column: x => x.MaLoai,
                        principalTable: "LoaiMon",
                        principalColumn: "MaLoai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaLamViec",
                columns: table => new
                {
                    MaCa = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GioBatDau = table.Column<TimeSpan>(type: "interval", nullable: false),
                    GioKetThuc = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MaNhanVien = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaLamViec", x => x.MaCa);
                    table.ForeignKey(
                        name: "FK_CaLamViec_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChamCong",
                columns: table => new
                {
                    MaChamCong = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaNhanVien = table.Column<int>(type: "integer", nullable: false),
                    NgayChamCong = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GioVao = table.Column<TimeSpan>(type: "interval", nullable: false),
                    GioRa = table.Column<TimeSpan>(type: "interval", nullable: false),
                    TrangThai = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChamCong", x => x.MaChamCong);
                    table.ForeignKey(
                        name: "FK_ChamCong_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GiaoDichKho",
                columns: table => new
                {
                    MaGiaoDich = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaNguyenLieu = table.Column<int>(type: "integer", nullable: false),
                    MaDonHang = table.Column<int>(type: "integer", nullable: true),
                    Loai = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SoLuong = table.Column<decimal>(type: "numeric", nullable: false),
                    NgayGio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LyDo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaoDichKho", x => x.MaGiaoDich);
                    table.ForeignKey(
                        name: "FK_GiaoDichKho_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang");
                    table.ForeignKey(
                        name: "FK_GiaoDichKho_Kho_MaNguyenLieu",
                        column: x => x.MaNguyenLieu,
                        principalTable: "Kho",
                        principalColumn: "MaNguyenLieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai_DonHang",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "integer", nullable: false),
                    MaKhuyenMai = table.Column<int>(type: "integer", nullable: false),
                    SoTienGiam = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai_DonHang", x => new { x.MaDonHang, x.MaKhuyenMai });
                    table.ForeignKey(
                        name: "FK_KhuyenMai_DonHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KhuyenMai_DonHang_KhuyenMai_MaKhuyenMai",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "MaKM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaDonHang = table.Column<int>(type: "integer", nullable: false),
                    MaMon = table.Column<int>(type: "integer", nullable: false),
                    SoLuong = table.Column<int>(type: "integer", nullable: false),
                    DonGia = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_ThucDon_MaMon",
                        column: x => x.MaMon,
                        principalTable: "ThucDon",
                        principalColumn: "MaMon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichLamViec",
                columns: table => new
                {
                    MaLichLamViec = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaNhanVien = table.Column<int>(type: "integer", nullable: false),
                    MaCa = table.Column<int>(type: "integer", nullable: false),
                    NgayLamViec = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichLamViec", x => x.MaLichLamViec);
                    table.ForeignKey(
                        name: "FK_LichLamViec_CaLamViec_MaCa",
                        column: x => x.MaCa,
                        principalTable: "CaLamViec",
                        principalColumn: "MaCa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichLamViec_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BoPhan",
                columns: new[] { "MaBoPhan", "TenBoPhan" },
                values: new object[] { 1, "Ban Quản Lý" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "MaVaiTro", "MoTa", "TenVaiTro" },
                values: new object[,]
                {
                    { 1, "Quản lý hệ thống", "QuanLy" },
                    { 2, "Nhân viên", "NhanVien" },
                    { 3, "Khách hàng", "KhachHang" }
                });

            migrationBuilder.InsertData(
                table: "NhanVien",
                columns: new[] { "MaNV", "ChucVu", "DiaChi", "Email", "HoTen", "MaBoPhan", "MatKhau", "SDT", "TrangThai" },
                values: new object[] { 1, "QuanLy", "Admin Address", "admin@gmail.com", "Admin", 1, "$2a$11$AHzAlAtAtx.m0g8q0x5nj.LB42KsSQx3hlzv7yE3UQGnn6oNhxIqm", "0123456789", "Đang làm việc" });

            migrationBuilder.CreateIndex(
                name: "IX_CaLamViec_MaNhanVien",
                table: "CaLamViec",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_ChamCong_MaNhanVien",
                table: "ChamCong",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaDonHang",
                table: "ChiTietDonHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaMon",
                table: "ChiTietDonHang",
                column: "MaMon");

            migrationBuilder.CreateIndex(
                name: "IX_DatBan_MaBan",
                table: "DatBan",
                column: "MaBan");

            migrationBuilder.CreateIndex(
                name: "IX_DatBan_MaKH",
                table: "DatBan",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_KhachHangID",
                table: "DonHang",
                column: "KhachHangID");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_NgayDat",
                table: "DonHang",
                column: "NgayDat");

            migrationBuilder.CreateIndex(
                name: "IX_GiaoDichKho_MaDonHang",
                table: "GiaoDichKho",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_GiaoDichKho_MaNguyenLieu",
                table: "GiaoDichKho",
                column: "MaNguyenLieu");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMai_NgayBatDau_NgayKetThuc",
                table: "KhuyenMai",
                columns: new[] { "NgayBatDau", "NgayKetThuc" });

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMai_DonHang_MaKhuyenMai",
                table: "KhuyenMai_DonHang",
                column: "MaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViec_MaCa",
                table: "LichLamViec",
                column: "MaCa");

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViec_MaNhanVien_NgayLamViec_MaCa",
                table: "LichLamViec",
                columns: new[] { "MaNhanVien", "NgayLamViec", "MaCa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_Email",
                table: "NhanVien",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_MaBoPhan",
                table: "NhanVien",
                column: "MaBoPhan");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_SDT",
                table: "NhanVien",
                column: "SDT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThucDon_KhuyenMaiMaKM",
                table: "ThucDon",
                column: "KhuyenMaiMaKM");

            migrationBuilder.CreateIndex(
                name: "IX_ThucDon_MaLoai",
                table: "ThucDon",
                column: "MaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_ThucDon_TenMon",
                table: "ThucDon",
                column: "TenMon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChamCong");

            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "DatBan");

            migrationBuilder.DropTable(
                name: "GiaoDichKho");

            migrationBuilder.DropTable(
                name: "KhuyenMai_DonHang");

            migrationBuilder.DropTable(
                name: "LichLamViec");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "ThucDon");

            migrationBuilder.DropTable(
                name: "Ban");

            migrationBuilder.DropTable(
                name: "Kho");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "CaLamViec");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "LoaiMon");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "BoPhan");
        }
    }
}
