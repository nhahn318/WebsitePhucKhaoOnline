using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsitePhucKhao.Migrations
{
    /// <inheritdoc />
    public partial class sendgmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaGuiEmail",
                table: "DonPhucKhaos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailSinhVien",
                table: "DonPhucKhaos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaGuiEmail",
                table: "DonPhucKhaos");

            migrationBuilder.DropColumn(
                name: "EmailSinhVien",
                table: "DonPhucKhaos");
        }
    }
}
