using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKhuyenMaiStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonAn_KhuyenMai_KhuyenMaiMaKhuyenMai",
                table: "MonAn");

            migrationBuilder.DropIndex(
                name: "IX_MonAn_KhuyenMaiMaKhuyenMai",
                table: "MonAn");

            migrationBuilder.DropColumn(
                name: "KhuyenMaiMaKhuyenMai",
                table: "MonAn");

            migrationBuilder.DropColumn(
                name: "MaKM",
                table: "MonAn");

            migrationBuilder.AddColumn<int>(
                name: "MonAnMaMon",
                table: "KhuyenMai_MonAn",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMai_MonAn_MonAnMaMon",
                table: "KhuyenMai_MonAn",
                column: "MonAnMaMon");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMai_MonAn_MonAn_MonAnMaMon",
                table: "KhuyenMai_MonAn",
                column: "MonAnMaMon",
                principalTable: "MonAn",
                principalColumn: "MaMon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMai_MonAn_MonAn_MonAnMaMon",
                table: "KhuyenMai_MonAn");

            migrationBuilder.DropIndex(
                name: "IX_KhuyenMai_MonAn_MonAnMaMon",
                table: "KhuyenMai_MonAn");

            migrationBuilder.DropColumn(
                name: "MonAnMaMon",
                table: "KhuyenMai_MonAn");

            migrationBuilder.AddColumn<int>(
                name: "KhuyenMaiMaKhuyenMai",
                table: "MonAn",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaKM",
                table: "MonAn",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonAn_KhuyenMaiMaKhuyenMai",
                table: "MonAn",
                column: "KhuyenMaiMaKhuyenMai");

            migrationBuilder.AddForeignKey(
                name: "FK_MonAn_KhuyenMai_KhuyenMaiMaKhuyenMai",
                table: "MonAn",
                column: "KhuyenMaiMaKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "MaKhuyenMai");
        }
    }
}
