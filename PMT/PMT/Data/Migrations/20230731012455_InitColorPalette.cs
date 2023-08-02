using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Data.Migrations;

/// <inheritdoc />
public partial class InitColorPalette : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ColorPalettes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjId = table.Column<int>(type: "int", nullable: false),
                Colors = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ColorPalettes", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ColorPalettes");
    }
}
