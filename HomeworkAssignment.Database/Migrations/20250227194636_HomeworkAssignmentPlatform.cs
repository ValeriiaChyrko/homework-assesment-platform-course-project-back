using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAssignment.Database.Migrations
{
    /// <inheritdoc />
    public partial class HomeworkAssignmentPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    RoleType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    GithubUsername = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    GithubProfileUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    GithubPictureUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseEntities_CategoryEntities_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChapterEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    VideoUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    IsFree = table.Column<bool>(type: "boolean", nullable: false),
                    MuxDataId = table.Column<Guid>(type: "uuid", nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterEntities_CourseEntities_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnrollmentEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrollmentEntities_CourseEntities_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnrollmentEntities_UserEntities_UserId",
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
                    RepositoryName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RepositoryOwner = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RepositoryUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false),
                    MaxAttemptsAmount = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    AttemptCompilationSectionEnable = table.Column<bool>(type: "boolean", nullable: false),
                    AttemptTestsSectionEnable = table.Column<bool>(type: "boolean", nullable: false),
                    AttemptQualitySectionEnable = table.Column<bool>(type: "boolean", nullable: false),
                    AttemptCompilationMaxScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptCompilationMinScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptTestsMaxScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptTestsMinScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptQualityMaxScore = table.Column<int>(type: "integer", nullable: false),
                    AttemptQualityMinScore = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentEntities_ChapterEntities_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ChapterEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentEntities_ChapterEntities_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ChapterEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentEntities_CourseEntities_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MuxDataEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PlaybackId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuxDataEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MuxDataEntities_ChapterEntities_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ChapterEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProgressEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgressEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProgressEntities_ChapterEntities_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ChapterEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProgressEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttemptProgressEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    BranchName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FinalScore = table.Column<int>(type: "integer", nullable: false),
                    CompilationScore = table.Column<int>(type: "integer", nullable: false),
                    QualityScore = table.Column<int>(type: "integer", nullable: false),
                    TestsScore = table.Column<int>(type: "integer", nullable: false),
                    ProgressStatus = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptProgressEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttemptProgressEntities_AssignmentEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "AssignmentEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttemptProgressEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentEntities_ChapterId",
                table: "AssignmentEntities",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentEntities_ChapterId",
                table: "AttachmentEntities",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentEntities_CourseId",
                table: "AttachmentEntities",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptProgressEntities_AssignmentId",
                table: "AttemptProgressEntities",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptProgressEntities_UserId",
                table: "AttemptProgressEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEntities_Name",
                table: "CategoryEntities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterEntities_CourseId",
                table: "ChapterEntities",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEntities_CategoryId",
                table: "CourseEntities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEntities_UserId",
                table: "CourseEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentEntities_CourseId",
                table: "EnrollmentEntities",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentEntities_UserId_CourseId",
                table: "EnrollmentEntities",
                columns: new[] { "UserId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MuxDataEntities_ChapterId",
                table: "MuxDataEntities",
                column: "ChapterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MuxDataEntities_PlaybackId",
                table: "MuxDataEntities",
                column: "PlaybackId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEntities_Email",
                table: "UserEntities",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEntities_GithubUsername",
                table: "UserEntities",
                column: "GithubUsername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgressEntities_ChapterId",
                table: "UserProgressEntities",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgressEntities_UserId_ChapterId",
                table: "UserProgressEntities",
                columns: new[] { "UserId", "ChapterId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentEntities");

            migrationBuilder.DropTable(
                name: "AttemptProgressEntities");

            migrationBuilder.DropTable(
                name: "EnrollmentEntities");

            migrationBuilder.DropTable(
                name: "MuxDataEntities");

            migrationBuilder.DropTable(
                name: "UserProgressEntities");

            migrationBuilder.DropTable(
                name: "AssignmentEntities");

            migrationBuilder.DropTable(
                name: "ChapterEntities");

            migrationBuilder.DropTable(
                name: "CourseEntities");

            migrationBuilder.DropTable(
                name: "CategoryEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");
        }
    }
}
