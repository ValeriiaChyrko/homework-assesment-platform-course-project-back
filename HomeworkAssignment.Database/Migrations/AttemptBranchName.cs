#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeAssignment.Database.Migrations;

/// <inheritdoc />
public partial class AttemptBranchName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "BranchName",
            "AttemptEntities",
            "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "BranchName",
            "AttemptEntities");
    }
}