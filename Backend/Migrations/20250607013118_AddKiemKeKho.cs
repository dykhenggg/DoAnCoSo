using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddKiemKeKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietKiemKho");

            migrationBuilder.DropTable(
                name: "KiemKho");

            migrationBuilder.CreateTable(
                name: "KiemKeKho",
                columns: table => new
                {
                    MaKiemKe = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayKiemKe = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NguoiKiemKe = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiemKeKho", x => x.MaKiemKe);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietKiemKe",
                columns: table => new
                {
                    MaChiTietKiemKe = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaKiemKe = table.Column<int>(type: "integer", nullable: false),
                    MaNguyenLieu = table.Column<int>(type: "integer", nullable: false),
                    SoLuongThucTe = table.Column<decimal>(type: "numeric", nullable: false),
                    ChenhLech = table.Column<decimal>(type: "numeric", nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietKiemKe", x => x.MaChiTietKiemKe);
                    table.ForeignKey(
                        name: "FK_ChiTietKiemKe_Kho_MaNguyenLieu",
                        column: x => x.MaNguyenLieu,
                        principalTable: "Kho",
                        principalColumn: "MaNguyenLieu",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietKiemKe_KiemKeKho_MaKiemKe",
                        column: x => x.MaKiemKe,
                        principalTable: "KiemKeKho",
                        principalColumn: "MaKiemKe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKiemKe_MaKiemKe",
                table: "ChiTietKiemKe",
                column: "MaKiemKe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKiemKe_MaNguyenLieu",
                table: "ChiTietKiemKe",
                column: "MaNguyenLieu");

            migrationBuilder.CreateIndex(
                name: "IX_KiemKeKho_NgayKiemKe",
                table: "KiemKeKho",
                column: "NgayKiemKe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietKiemKe");

            migrationBuilder.DropTable(
                name: "KiemKeKho");

            migrationBuilder.CreateTable(
                name: "KiemKho",
                columns: table => new
                {
                    MaKiemKho = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaNhanVien = table.Column<int>(type: "integer", nullable: false),
                    GhiChu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    NgayKiem = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TrangThai = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiemKho", x => x.MaKiemKho);
                    table.ForeignKey(
                        name: "FK_KiemKho_NhanVien_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietKiemKho",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaKiemKho = table.Column<int>(type: "integer", nullable: false),
                    MaNguyenLieu = table.Column<int>(type: "integer", nullable: false),
                    ChenhLech = table.Column<decimal>(type: "numeric", nullable: false),
                    LyDo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SoLuongHeThong = table.Column<decimal>(type: "numeric", nullable: false),
                    SoLuongThucTe = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietKiemKho", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietKiemKho_Kho_MaNguyenLieu",
                        column: x => x.MaNguyenLieu,
                        principalTable: "Kho",
                        principalColumn: "MaNguyenLieu",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietKiemKho_KiemKho_MaKiemKho",
                        column: x => x.MaKiemKho,
                        principalTable: "KiemKho",
                        principalColumn: "MaKiemKho",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKiemKho_MaKiemKho",
                table: "ChiTietKiemKho",
                column: "MaKiemKho");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKiemKho_MaNguyenLieu",
                table: "ChiTietKiemKho",
                column: "MaNguyenLieu");

            migrationBuilder.CreateIndex(
                name: "IX_KiemKho_MaNhanVien",
                table: "KiemKho",
                column: "MaNhanVien");
        }
    }
}
