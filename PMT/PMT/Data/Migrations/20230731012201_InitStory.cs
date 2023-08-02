using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Data.Migrations;

/// <inheritdoc />
public partial class InitStory : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Stories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                DateResolved = table.Column<DateTime>(type: "datetime2", nullable: true),
                DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Stories", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Stories");
    }
}
