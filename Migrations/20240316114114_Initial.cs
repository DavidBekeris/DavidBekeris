using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DavidBekeris.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndexModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    heroUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    heroText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    heroHeader = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjektModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlSlug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjektModels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndexModels");

            migrationBuilder.DropTable(
                name: "ProjektModels");
        }
    }
}
