using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Migrations;

/// <inheritdoc />
public partial class HoursToMilli : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Hours",
            table: "TimeSets",
            newName: "Milliseconds");

        migrationBuilder.RenameColumn(
            name: "Hours",
            table: "TimeIntervals",
            newName: "Milliseconds");

        migrationBuilder.RenameColumn(
            name: "TotalHours",
            table: "Stopwatches",
            newName: "Milliseconds");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Milliseconds",
            table: "TimeSets",
            newName: "Hours");

        migrationBuilder.RenameColumn(
            name: "Milliseconds",
            table: "TimeIntervals",
            newName: "Hours");

        migrationBuilder.RenameColumn(
            name: "Milliseconds",
            table: "Stopwatches",
            newName: "TotalHours");
    }
}
