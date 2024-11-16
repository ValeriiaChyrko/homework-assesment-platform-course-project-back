using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAssignment.Database.Migrations
{
    /// <inheritdoc />
    public partial class AttemptBranchName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "AttemptEntities",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "AttemptEntities");
        }
    }
}
