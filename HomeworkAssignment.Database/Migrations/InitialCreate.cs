#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeAssignment.Database.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "UserEntities",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                FullName = table.Column<string>("character varying(128)", maxLength: 128, nullable: false),
                Email = table.Column<string>("character varying(254)", maxLength: 254, nullable: false),
                PasswordHash = table.Column<string>("character varying(254)", maxLength: 254, nullable: false),
                RoleType = table.Column<string>("character varying(32)", maxLength: 32, nullable: false),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_UserEntities", x => x.Id); });

        migrationBuilder.CreateTable(
            "GitHubProfilesEntities",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                GithubUsername = table.Column<string>("character varying(64)", maxLength: 64, nullable: false),
                GithubAccessToken = table.Column<string>("character varying(128)", maxLength: 128, nullable: false),
                GithubProfileUrl = table.Column<string>("character varying(128)", maxLength: 128, nullable: false),
                GithubPictureUrl = table.Column<string>("character varying(128)", maxLength: 128, nullable: true),
                UserId = table.Column<Guid>("uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GitHubProfilesEntities", x => x.Id);
                table.ForeignKey(
                    "FK_GitHubProfilesEntities_UserEntities_UserId",
                    x => x.UserId,
                    "UserEntities",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AssignmentEntities",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Title = table.Column<string>("character varying(64)", maxLength: 64, nullable: false),
                Description = table.Column<string>("character varying(512)", maxLength: 512, nullable: true),
                Deadline = table.Column<DateTime>("timestamp with time zone", nullable: false),
                MaxScore = table.Column<int>("integer", nullable: false),
                MaxAttemptsAmount = table.Column<int>("integer", nullable: false),
                AttemptCompilationSectionEnable = table.Column<bool>("boolean", nullable: false),
                AttemptTestsSectionEnable = table.Column<bool>("boolean", nullable: false),
                AttemptQualitySectionEnable = table.Column<bool>("boolean", nullable: false),
                AttemptCompilationMaxScore = table.Column<int>("integer", nullable: false),
                AttemptCompilationMinScore = table.Column<int>("integer", nullable: false),
                AttemptTestsMaxScore = table.Column<int>("integer", nullable: false),
                AttemptTestsMinScore = table.Column<int>("integer", nullable: false),
                AttemptQualityMaxScore = table.Column<int>("integer", nullable: false),
                AttemptQualityMinScore = table.Column<int>("integer", nullable: false),
                OwnerId = table.Column<Guid>("uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AssignmentEntities", x => x.Id);
                table.ForeignKey(
                    "FK_AssignmentEntities_GitHubProfilesEntities_OwnerId",
                    x => x.OwnerId,
                    "GitHubProfilesEntities",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "AttemptEntities",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                AttemptNumber = table.Column<int>("integer", nullable: false),
                FinishedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                CompilationScore = table.Column<int>("integer", nullable: false),
                TestsScore = table.Column<int>("integer", nullable: false),
                QualityScore = table.Column<int>("integer", nullable: false),
                FinalScore = table.Column<int>("integer", nullable: false),
                StudentId = table.Column<Guid>("uuid", nullable: false),
                AssignmentId = table.Column<Guid>("uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AttemptEntities", x => x.Id);
                table.ForeignKey(
                    "FK_AttemptEntities_AssignmentEntities_AssignmentId",
                    x => x.AssignmentId,
                    "AssignmentEntities",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_AttemptEntities_GitHubProfilesEntities_StudentId",
                    x => x.StudentId,
                    "GitHubProfilesEntities",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_AssignmentEntities_OwnerId",
            "AssignmentEntities",
            "OwnerId");

        migrationBuilder.CreateIndex(
            "IX_AttemptEntities_AssignmentId",
            "AttemptEntities",
            "AssignmentId");

        migrationBuilder.CreateIndex(
            "IX_AttemptEntities_StudentId",
            "AttemptEntities",
            "StudentId");

        migrationBuilder.CreateIndex(
            "IX_GitHubProfilesEntities_UserId",
            "GitHubProfilesEntities",
            "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "AttemptEntities");

        migrationBuilder.DropTable(
            "AssignmentEntities");

        migrationBuilder.DropTable(
            "GitHubProfilesEntities");

        migrationBuilder.DropTable(
            "UserEntities");
    }
}