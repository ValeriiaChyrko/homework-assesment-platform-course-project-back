using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAssignment.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    RoleType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GitHubProfilesEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GithubUsername = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    GithubAccessToken = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    GithubProfileUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    GithubPictureUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubProfilesEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitHubProfilesEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false),
                    MaxAttemptsAmount = table.Column<int>(type: "integer", nullable: false),
                    AttemptCompilationSectionEnable = table.Column<bool>(type: "boolean", nullable: false),
                    AttemptTestsSectionEnable = table.Column<bool>(type: "boolean", nullable: false),
                    AttemptQualitySectionEnable = table.Column<bool>(type: "boolean", nullable: false),
                    AttemptCompilationMaxScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptCompilationMinScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptTestsMaxScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptTestsMinScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptQualityMaxScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptQualityMinScore = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentEntities_GitHubProfilesEntities_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "GitHubProfilesEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttemptEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompilationScore = table.Column<int>(type: "integer", nullable: false),
                    TestsScore = table.Column<int>(type: "integer", nullable: false),
                    QualityScore = table.Column<int>(type: "integer", nullable: false),
                    FinalScore = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttemptEntities_AssignmentEntities_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "AssignmentEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttemptEntities_GitHubProfilesEntities_StudentId",
                        column: x => x.StudentId,
                        principalTable: "GitHubProfilesEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentEntities_OwnerId",
                table: "AssignmentEntities",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptEntities_AssignmentId",
                table: "AttemptEntities",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptEntities_StudentId",
                table: "AttemptEntities",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GitHubProfilesEntities_UserId",
                table: "GitHubProfilesEntities",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttemptEntities");

            migrationBuilder.DropTable(
                name: "AssignmentEntities");

            migrationBuilder.DropTable(
                name: "GitHubProfilesEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");
        }
    }
}
