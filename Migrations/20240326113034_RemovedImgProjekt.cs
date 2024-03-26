using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DavidBekeris.Migrations
{
    /// <inheritdoc />
    public partial class RemovedImgProjekt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProjektModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProjektModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
