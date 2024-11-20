#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeAssignment.Database.Migrations;

/// <inheritdoc />
public partial class RepositoryName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "GithubAccessToken",
            "GitHubProfilesEntities");

        migrationBuilder.AddColumn<string>(
            "RepositoryName",
            "AssignmentEntities",
            "character varying(64)",
            maxLength: 64,
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "RepositoryName",
            "AssignmentEntities");

        migrationBuilder.AddColumn<string>(
            "GithubAccessToken",
            "GitHubProfilesEntities",
            "character varying(128)",
            maxLength: 128,
            nullable: false,
            defaultValue: "");
    }
}