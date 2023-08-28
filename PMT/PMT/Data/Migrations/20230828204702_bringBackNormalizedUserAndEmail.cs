using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMT.Data.Migrations;

/// <inheritdoc />
public partial class bringBackNormalizedUserAndEmail : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "NormalizedEmail",
            table: "AspNetUsers",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NormalizedUserName",
            table: "AspNetUsers",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AspNetUsers",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "EmailIndex",
            table: "AspNetUsers");

        migrationBuilder.DropIndex(
            name: "UserNameIndex",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "NormalizedEmail",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "NormalizedUserName",
            table: "AspNetUsers");
    }
}
