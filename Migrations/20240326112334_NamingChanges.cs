using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DavidBekeris.Migrations
{
    /// <inheritdoc />
    public partial class NamingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "heroUrl",
                table: "IndexModels",
                newName: "HeroUrl");

            migrationBuilder.RenameColumn(
                name: "heroText",
                table: "IndexModels",
                newName: "HeroText");

            migrationBuilder.RenameColumn(
                name: "heroHeader",
                table: "IndexModels",
                newName: "HeroHeader");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HeroUrl",
                table: "IndexModels",
                newName: "heroUrl");

            migrationBuilder.RenameColumn(
                name: "HeroText",
                table: "IndexModels",
                newName: "heroText");

            migrationBuilder.RenameColumn(
                name: "HeroHeader",
                table: "IndexModels",
                newName: "heroHeader");
        }
    }
}
