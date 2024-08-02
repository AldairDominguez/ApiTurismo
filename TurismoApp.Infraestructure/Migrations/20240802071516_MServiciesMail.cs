using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurismoApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class MServiciesMail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Clientes");
        }
    }
}
