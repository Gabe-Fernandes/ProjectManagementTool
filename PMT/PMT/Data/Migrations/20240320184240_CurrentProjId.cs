using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Migrations;

/// <inheritdoc />
public partial class CurrentProjId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "CurrentProjId",
            table: "AspNetUsers",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CurrentProjId",
            table: "AspNetUsers");
    }
}
