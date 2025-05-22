using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMoTaToBoPhan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "BoPhan",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BoPhan",
                keyColumn: "MaBoPhan",
                keyValue: 1,
                column: "MoTa",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "BoPhan");
        }
    }
}
