using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Data.Migrations;

/// <inheritdoc />
public partial class InitBugReport : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "BugReports",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RecreateIssue = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                AttemptedSolutions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                SuccessfulSolution = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                DateResolved = table.Column<DateTime>(type: "datetime2", nullable: true),
                DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BugReports", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BugReports");
    }
}
