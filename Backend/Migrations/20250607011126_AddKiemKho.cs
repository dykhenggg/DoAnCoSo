using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddKiemKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KiemKho",
                columns: table => new
                {
                    MaKiemKho = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayKiem = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaNhanVien = table.Column<int>(type: "integer", nullable: false),
                    TrangThai = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GhiChu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
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
                    SoLuongThucTe = table.Column<decimal>(type: "numeric", nullable: false),
                    SoLuongHeThong = table.Column<decimal>(type: "numeric", nullable: false),
                    ChenhLech = table.Column<decimal>(type: "numeric", nullable: false),
                    LyDo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietKiemKho");

            migrationBuilder.DropTable(
                name: "KiemKho");
        }
    }
}
