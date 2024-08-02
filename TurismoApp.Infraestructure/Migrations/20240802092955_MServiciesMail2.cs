using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurismoApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class MServiciesMail2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "Clientes");
        }
    }
}
