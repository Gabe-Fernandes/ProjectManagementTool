using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Migrations;

/// <inheritdoc />
public partial class TimeTrackerRework : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Shifts");

        migrationBuilder.AddColumn<double>(
            name: "TotalHours",
            table: "Stopwatches",
            type: "float",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.CreateTable(
            name: "TimeIntervals",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjId = table.Column<int>(type: "int", nullable: false),
                StopwatchId = table.Column<int>(type: "int", nullable: false),
                TimeSetId = table.Column<int>(type: "int", nullable: false),
                AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Hours = table.Column<double>(type: "float", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TimeIntervals", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "TimeSets",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjId = table.Column<int>(type: "int", nullable: false),
                AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                StopwatchId = table.Column<int>(type: "int", nullable: false),
                Hours = table.Column<double>(type: "float", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TimeSets", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "TimeIntervals");

        migrationBuilder.DropTable(
            name: "TimeSets");

        migrationBuilder.DropColumn(
            name: "TotalHours",
            table: "Stopwatches");

        migrationBuilder.CreateTable(
            name: "Shifts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClockIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                ClockOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Hours = table.Column<int>(type: "int", nullable: false),
                ProjId = table.Column<int>(type: "int", nullable: false),
                StopwatchId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Shifts", x => x.Id);
            });
    }
}
