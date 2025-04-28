using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

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
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SucChua = table.Column<int>(type: "integer", nullable: false),
                    TrangThai = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ban", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKhachHang = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HoTen = table.Column<string>(type: "text", nullable: false),
                    SoDienThoai = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
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
                    MaKhuyenMai = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ten = table.Column<string>(type: "text", nullable: false),
                    Loai = table.Column<string>(type: "text", nullable: false),
                    GiaTriGiam = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.MaKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "ThucDon",
                columns: table => new
                {
                    MaMon = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenMon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<decimal>(type: "numeric", nullable: false),
                    LoaiMon = table.Column<string>(type: "text", nullable: false),
                    HinhAnh = table.Column<string>(type: "text", nullable: false),
                    TrangThai = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThucDon", x => x.MaMon);
                });

            migrationBuilder.CreateTable(
                name: "VaiTro",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenVaiTro = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTro", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "DatBan",
                columns: table => new
                {
                    MaDatBan = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaKhachHang = table.Column<int>(type: "integer", nullable: false),
                    MaBan = table.Column<int>(type: "integer", nullable: false),
                    ThoiGianDat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SoNguoi = table.Column<int>(type: "integer", nullable: false),
                    TrangThai = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatBan", x => x.MaDatBan);
                    table.ForeignKey(
                        name: "FK_DatBan_Ban_MaBan",
                        column: x => x.MaBan,
                        principalTable: "Ban",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DatBan_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
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
                    ThucDonID = table.Column<int>(type: "integer", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TrangThai = table.Column<string>(type: "text", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_DonHang_ThucDon_ThucDonID",
                        column: x => x.ThucDonID,
                        principalTable: "ThucDon",
                        principalColumn: "MaMon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNhanVien = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaVaiTro = table.Column<int>(type: "integer", nullable: false),
                    Ten = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ChucVu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SoDienThoai = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TenDangNhap = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.MaNhanVien);
                    table.ForeignKey(
                        name: "FK_NhanVien_VaiTro_MaVaiTro",
                        column: x => x.MaVaiTro,
                        principalTable: "VaiTro",
                        principalColumn: "MaVaiTro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CT_DonHang",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "integer", nullable: false),
                    MaMon = table.Column<int>(type: "integer", nullable: false),
                    SoLuong = table.Column<int>(type: "integer", nullable: false),
                    GiaTaiThoiDiem = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_DonHang", x => new { x.MaDonHang, x.MaMon });
                    table.ForeignKey(
                        name: "FK_CT_DonHang_DonHang_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CT_DonHang_ThucDon_MaMon",
                        column: x => x.MaMon,
                        principalTable: "ThucDon",
                        principalColumn: "MaMon",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "MaKhuyenMai",
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
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChamCong",
                columns: table => new
                {
                    MaChamCong = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaNhanVien = table.Column<int>(type: "integer", nullable: false),
                    Ngay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichLamViec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaNhanVien = table.Column<int>(type: "integer", nullable: false),
                    MaCa = table.Column<int>(type: "integer", nullable: false),
                    NgayLamViec = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichLamViec", x => x.Id);
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
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaLamViec_MaNhanVien",
                table: "CaLamViec",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_ChamCong_MaNhanVien",
                table: "ChamCong",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_CT_DonHang_MaMon",
                table: "CT_DonHang",
                column: "MaMon");

            migrationBuilder.CreateIndex(
                name: "IX_DatBan_MaBan",
                table: "DatBan",
                column: "MaBan");

            migrationBuilder.CreateIndex(
                name: "IX_DatBan_MaKhachHang",
                table: "DatBan",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_KhachHangID",
                table: "DonHang",
                column: "KhachHangID");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_ThucDonID",
                table: "DonHang",
                column: "ThucDonID");

            migrationBuilder.CreateIndex(
                name: "IX_GiaoDichKho_MaDonHang",
                table: "GiaoDichKho",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_GiaoDichKho_MaNguyenLieu",
                table: "GiaoDichKho",
                column: "MaNguyenLieu");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMai_DonHang_MaKhuyenMai",
                table: "KhuyenMai_DonHang",
                column: "MaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViec_MaCa",
                table: "LichLamViec",
                column: "MaCa");

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViec_MaNhanVien",
                table: "LichLamViec",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_MaVaiTro",
                table: "NhanVien",
                column: "MaVaiTro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChamCong");

            migrationBuilder.DropTable(
                name: "CT_DonHang");

            migrationBuilder.DropTable(
                name: "DatBan");

            migrationBuilder.DropTable(
                name: "GiaoDichKho");

            migrationBuilder.DropTable(
                name: "KhuyenMai_DonHang");

            migrationBuilder.DropTable(
                name: "LichLamViec");

            migrationBuilder.DropTable(
                name: "Ban");

            migrationBuilder.DropTable(
                name: "Kho");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "CaLamViec");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "ThucDon");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "VaiTro");
        }
    }
}
