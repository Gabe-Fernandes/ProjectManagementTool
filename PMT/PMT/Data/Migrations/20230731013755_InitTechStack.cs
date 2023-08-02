using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Data.Migrations;

/// <inheritdoc />
public partial class InitTechStack : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "TechStacks",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjId = table.Column<int>(type: "int", nullable: false),
                SourceControl = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                BackendFramework = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                BackendLanguage = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                FrontendFramework = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                FrontendLanguage = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                Styling = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                Database = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                ORM = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                UIDesign = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                Deployment = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TechStacks", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "TechStacks");
    }
}
