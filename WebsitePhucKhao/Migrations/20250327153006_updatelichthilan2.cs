using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsitePhucKhao.Migrations
{
    /// <inheritdoc />
    public partial class updatelichthilan2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaNamHoc",
                table: "LichThis",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LichThis_MaNamHoc",
                table: "LichThis",
                column: "MaNamHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_LichThis_NamHocs_MaNamHoc",
                table: "LichThis",
                column: "MaNamHoc",
                principalTable: "NamHocs",
                principalColumn: "MaNamHoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LichThis_NamHocs_MaNamHoc",
                table: "LichThis");

            migrationBuilder.DropIndex(
                name: "IX_LichThis_MaNamHoc",
                table: "LichThis");

            migrationBuilder.DropColumn(
                name: "MaNamHoc",
                table: "LichThis");
        }
    }
}
