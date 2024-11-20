#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeAssignment.Database.Migrations;

/// <inheritdoc />
public partial class Indexing : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            "IX_UserEntities_Email",
            "UserEntities",
            "Email");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            "IX_UserEntities_Email",
            "UserEntities");
    }
}