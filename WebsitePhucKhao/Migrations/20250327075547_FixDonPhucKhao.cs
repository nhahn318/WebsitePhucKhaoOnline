using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsitePhucKhao.Migrations
{
    /// <inheritdoc />
    public partial class FixDonPhucKhao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonPhucKhaoChiTiets_DonPhucKhaos_DonPhucKhaoMaDon",
                table: "DonPhucKhaoChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_DonPhucKhaoChiTiets_DonPhucKhaoMaDon",
                table: "DonPhucKhaoChiTiets");

            migrationBuilder.DropColumn(
                name: "DonPhucKhaoMaDon",
                table: "DonPhucKhaoChiTiets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DonPhucKhaoMaDon",
                table: "DonPhucKhaoChiTiets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonPhucKhaoChiTiets_DonPhucKhaoMaDon",
                table: "DonPhucKhaoChiTiets",
                column: "DonPhucKhaoMaDon");

            migrationBuilder.AddForeignKey(
                name: "FK_DonPhucKhaoChiTiets_DonPhucKhaos_DonPhucKhaoMaDon",
                table: "DonPhucKhaoChiTiets",
                column: "DonPhucKhaoMaDon",
                principalTable: "DonPhucKhaos",
                principalColumn: "MaDon");
        }
    }
}
