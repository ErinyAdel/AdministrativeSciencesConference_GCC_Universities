using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saudi_FormEmail.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "ContactFormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ContactFormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "ContactFormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ContactFormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ContactFormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "ContactFormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ContactFormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "ContactFormSubmissions");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "ContactFormSubmissions");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "ContactFormSubmissions");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "ContactFormSubmissions");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ContactFormSubmissions");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "ContactFormSubmissions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ContactFormSubmissions");
        }
    }
}
