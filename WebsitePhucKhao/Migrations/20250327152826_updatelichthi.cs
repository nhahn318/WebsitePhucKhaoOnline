using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsitePhucKhao.Migrations
{
    /// <inheritdoc />
    public partial class updatelichthi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaThi",
                table: "LichThis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaDiemThi",
                table: "LichThis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhongThi",
                table: "LichThis",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaThi",
                table: "LichThis");

            migrationBuilder.DropColumn(
                name: "DiaDiemThi",
                table: "LichThis");

            migrationBuilder.DropColumn(
                name: "PhongThi",
                table: "LichThis");
        }
    }
}
