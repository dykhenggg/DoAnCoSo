using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDatBanMonAn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatBanMonAn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaDatBan = table.Column<int>(type: "integer", nullable: false),
                    MaMon = table.Column<int>(type: "integer", nullable: false),
                    SoLuong = table.Column<int>(type: "integer", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NguoiTao = table.Column<string>(type: "text", nullable: true),
                    NguoiCapNhat = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatBanMonAn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatBanMonAn_DatBan_MaDatBan",
                        column: x => x.MaDatBan,
                        principalTable: "DatBan",
                        principalColumn: "MaDatBan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DatBanMonAn_MonAn_MaMon",
                        column: x => x.MaMon,
                        principalTable: "MonAn",
                        principalColumn: "MaMon",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatBanMonAn_MaDatBan",
                table: "DatBanMonAn",
                column: "MaDatBan");

            migrationBuilder.CreateIndex(
                name: "IX_DatBanMonAn_MaMon",
                table: "DatBanMonAn",
                column: "MaMon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatBanMonAn");
        }
    }
}
