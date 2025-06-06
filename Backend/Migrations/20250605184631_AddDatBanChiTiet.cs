using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDatBanChiTiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GiamGia",
                table: "DatBan",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PhuongThucThanhToan",
                table: "DatBan",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ThanhTien",
                table: "DatBan",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianThanhToan",
                table: "DatBan",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTien",
                table: "DatBan",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "DatBan",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DatBanChiTiet",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaDatBan = table.Column<int>(type: "integer", nullable: false),
                    MaMon = table.Column<int>(type: "integer", nullable: false),
                    SoLuong = table.Column<int>(type: "integer", nullable: false),
                    DonGia = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatBanChiTiet", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_DatBanChiTiet_DatBan_MaDatBan",
                        column: x => x.MaDatBan,
                        principalTable: "DatBan",
                        principalColumn: "MaDatBan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DatBanChiTiet_MonAn_MaMon",
                        column: x => x.MaMon,
                        principalTable: "MonAn",
                        principalColumn: "MaMon",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatBanChiTiet_MaDatBan",
                table: "DatBanChiTiet",
                column: "MaDatBan");

            migrationBuilder.CreateIndex(
                name: "IX_DatBanChiTiet_MaMon",
                table: "DatBanChiTiet",
                column: "MaMon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatBanChiTiet");

            migrationBuilder.DropColumn(
                name: "GiamGia",
                table: "DatBan");

            migrationBuilder.DropColumn(
                name: "PhuongThucThanhToan",
                table: "DatBan");

            migrationBuilder.DropColumn(
                name: "ThanhTien",
                table: "DatBan");

            migrationBuilder.DropColumn(
                name: "ThoiGianThanhToan",
                table: "DatBan");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "DatBan");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "DatBan");
        }
    }
}
