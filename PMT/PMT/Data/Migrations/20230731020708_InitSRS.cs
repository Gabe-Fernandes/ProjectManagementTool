using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Data.Migrations;

/// <inheritdoc />
public partial class InitSRS : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SRSs",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjId = table.Column<int>(type: "int", nullable: false),
                DomainName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                TargetDemographic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SRSs", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SRSs");
    }
}
