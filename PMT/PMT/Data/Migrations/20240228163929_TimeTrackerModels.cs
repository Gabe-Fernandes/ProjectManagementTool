using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Migrations;

/// <inheritdoc />
public partial class TimeTrackerModels : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Title",
            table: "Stories",
            type: "nvarchar(45)",
            maxLength: 45,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(30)",
            oldMaxLength: 30);

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Stories",
            type: "nvarchar(3000)",
            maxLength: 3000,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(600)",
            oldMaxLength: 600);

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "BugReports",
            type: "nvarchar(3000)",
            maxLength: 3000,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(600)",
            oldMaxLength: 600);

        migrationBuilder.CreateTable(
            name: "Shifts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjId = table.Column<int>(type: "int", nullable: false),
                StopwatchId = table.Column<int>(type: "int", nullable: false),
                AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                ClockIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                ClockOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                Hours = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Shifts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Stopwatches",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjId = table.Column<int>(type: "int", nullable: false),
                AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Stopwatches", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Shifts");

        migrationBuilder.DropTable(
            name: "Stopwatches");

        migrationBuilder.AlterColumn<string>(
            name: "Title",
            table: "Stories",
            type: "nvarchar(30)",
            maxLength: 30,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(45)",
            oldMaxLength: 45);

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Stories",
            type: "nvarchar(600)",
            maxLength: 600,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(3000)",
            oldMaxLength: 3000);

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "BugReports",
            type: "nvarchar(600)",
            maxLength: 600,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(3000)",
            oldMaxLength: 3000);
    }
}
