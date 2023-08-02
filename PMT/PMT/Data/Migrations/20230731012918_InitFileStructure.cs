using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Data.Migrations;

/// <inheritdoc />
public partial class InitFileStructure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FileStructures",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjId = table.Column<int>(type: "int", nullable: false),
                FileStructureData = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FileStructures", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FileStructures");
    }
}
