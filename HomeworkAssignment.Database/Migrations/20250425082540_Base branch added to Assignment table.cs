using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAssignment.Database.Migrations
{
    /// <inheritdoc />
    public partial class BasebranchaddedtoAssignmenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RepositoryBaseBranchName",
                table: "AssignmentEntities",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepositoryBaseBranchName",
                table: "AssignmentEntities");
        }
    }
}
