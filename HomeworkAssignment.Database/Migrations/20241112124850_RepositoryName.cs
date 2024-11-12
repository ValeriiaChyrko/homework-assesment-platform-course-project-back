using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAssignment.Database.Migrations
{
    /// <inheritdoc />
    public partial class RepositoryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubAccessToken",
                table: "GitHubProfilesEntities");

            migrationBuilder.AddColumn<string>(
                name: "RepositoryName",
                table: "AssignmentEntities",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepositoryName",
                table: "AssignmentEntities");

            migrationBuilder.AddColumn<string>(
                name: "GithubAccessToken",
                table: "GitHubProfilesEntities",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
