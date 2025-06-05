using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhoneNumberFromEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_DonHang_MaDonHang",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_BoPhan_MaBoPhan",
                table: "NhanVien");

            migrationBuilder.DropIndex(
                name: "IX_NhanVien_SDT",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "SDT",
                table: "NhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_DonHang_MaDonHang",
                table: "ChiTietDonHang",
                column: "MaDonHang",
                principalTable: "DonHang",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_BoPhan_MaBoPhan",
                table: "NhanVien",
                column: "MaBoPhan",
                principalTable: "BoPhan",
                principalColumn: "MaBoPhan",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_DonHang_MaDonHang",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_BoPhan_MaBoPhan",
                table: "NhanVien");

            migrationBuilder.AddColumn<string>(
                name: "SDT",
                table: "NhanVien",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_SDT",
                table: "NhanVien",
                column: "SDT",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_DonHang_MaDonHang",
                table: "ChiTietDonHang",
                column: "MaDonHang",
                principalTable: "DonHang",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_BoPhan_MaBoPhan",
                table: "NhanVien",
                column: "MaBoPhan",
                principalTable: "BoPhan",
                principalColumn: "MaBoPhan",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
