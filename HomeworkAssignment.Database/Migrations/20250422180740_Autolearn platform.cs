using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeAssignment.Database.Migrations
{
    /// <inheritdoc />
    public partial class Autolearnplatform : Migration
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
                name: "RoleEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
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
                name: "UserRolesEntities",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolesEntities", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRolesEntities_RoleEntities_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RoleEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRolesEntities_UserEntities_UserId",
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
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
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
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    Description = table.Column<string>(type: "character varying(15000)", maxLength: 15000, nullable: true),
                    RepositoryName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    RepositoryOwner = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    RepositoryUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: true)
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
                    UploadthingKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                name: "UserChapterProgressEntities",
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
                    table.PrimaryKey("PK_UserChapterProgressEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChapterProgressEntities_ChapterEntities_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ChapterEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChapterProgressEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttemptEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    BranchName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    FinalScore = table.Column<int>(type: "integer", nullable: false),
                    CompilationScore = table.Column<int>(type: "integer", nullable: false),
                    QualityScore = table.Column<int>(type: "integer", nullable: false),
                    TestsScore = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "FK_AttemptEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAssignmentProgressEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssignmentProgressEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAssignmentProgressEntities_AssignmentEntities_Assignmen~",
                        column: x => x.AssignmentId,
                        principalTable: "AssignmentEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAssignmentProgressEntities_UserEntities_UserId",
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
                name: "IX_AttemptEntities_AssignmentId",
                table: "AttemptEntities",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptEntities_UserId",
                table: "AttemptEntities",
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
                name: "IX_UserAssignmentProgressEntities_AssignmentId",
                table: "UserAssignmentProgressEntities",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignmentProgressEntities_UserId",
                table: "UserAssignmentProgressEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapterProgressEntities_ChapterId",
                table: "UserChapterProgressEntities",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapterProgressEntities_UserId_ChapterId",
                table: "UserChapterProgressEntities",
                columns: new[] { "UserId", "ChapterId" },
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
                name: "IX_UserRolesEntities_RoleId",
                table: "UserRolesEntities",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentEntities");

            migrationBuilder.DropTable(
                name: "AttemptEntities");

            migrationBuilder.DropTable(
                name: "EnrollmentEntities");

            migrationBuilder.DropTable(
                name: "UserAssignmentProgressEntities");

            migrationBuilder.DropTable(
                name: "UserChapterProgressEntities");

            migrationBuilder.DropTable(
                name: "UserRolesEntities");

            migrationBuilder.DropTable(
                name: "AssignmentEntities");

            migrationBuilder.DropTable(
                name: "RoleEntities");

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
