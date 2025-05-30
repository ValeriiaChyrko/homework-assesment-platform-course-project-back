﻿// <auto-generated />
using System;
using HomeAssignment.Database.Contexts.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeAssignment.Database.Migrations
{
    [DbContext(typeof(HomeworkAssignmentDbContext))]
    [Migration("20250425082540_Base branch added to Assignment table")]
    partial class BasebranchaddedtoAssignmenttable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HomeAssignment.Database.Entities.AssignmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AttemptCompilationMaxScore")
                        .HasColumnType("integer");

                    b.Property<int>("AttemptCompilationMinScore")
                        .HasColumnType("integer");

                    b.Property<bool>("AttemptCompilationSectionEnable")
                        .HasColumnType("boolean");

                    b.Property<int>("AttemptQualityMaxScore")
                        .HasColumnType("integer");

                    b.Property<int>("AttemptQualityMinScore")
                        .HasColumnType("integer");

                    b.Property<bool>("AttemptQualitySectionEnable")
                        .HasColumnType("boolean");

                    b.Property<int>("AttemptTestsMaxScore")
                        .HasColumnType("integer");

                    b.Property<int>("AttemptTestsMinScore")
                        .HasColumnType("integer");

                    b.Property<bool>("AttemptTestsSectionEnable")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ChapterId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(15000)
                        .HasColumnType("character varying(15000)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxAttemptsAmount")
                        .HasColumnType("integer");

                    b.Property<int>("MaxScore")
                        .HasColumnType("integer");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<string>("RepositoryBaseBranchName")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("RepositoryName")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("RepositoryOwner")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("RepositoryUrl")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.ToTable("AssignmentEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.AttachmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ChapterId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UploadthingKey")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.HasIndex("CourseId");

                    b.ToTable("AttachmentEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.AttemptEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AssignmentId")
                        .HasColumnType("uuid");

                    b.Property<string>("BranchName")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("CompilationScore")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FinalScore")
                        .HasColumnType("integer");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("QualityScore")
                        .HasColumnType("integer");

                    b.Property<int>("TestsScore")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("UserId");

                    b.ToTable("AttemptEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("CategoryEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.ChapterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(10000)
                        .HasColumnType("character varying(10000)");

                    b.Property<bool>("IsFree")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("MuxDataId")
                        .HasColumnType("uuid");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("VideoUrl")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("ChapterEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.CourseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("CourseEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.EnrollmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId", "CourseId")
                        .IsUnique();

                    b.ToTable("EnrollmentEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RoleEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.UserAssignmentProgressEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AssignmentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAssignmentProgressEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.UserChapterProgressEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ChapterId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.HasIndex("UserId", "ChapterId")
                        .IsUnique();

                    b.ToTable("UserChapterProgressEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("GithubPictureUrl")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("GithubProfileUrl")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("GithubUsername")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("GithubUsername")
                        .IsUnique();

                    b.ToTable("UserEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.UserRolesEntity", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRolesEntities");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.AssignmentEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.ChapterEntity", "Chapter")
                        .WithMany("Assignments")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Chapter");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.AttachmentEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.ChapterEntity", "Chapter")
                        .WithMany("Attachments")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HomeAssignment.Database.Entities.CourseEntity", "Course")
                        .WithMany("Attachments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Chapter");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.AttemptEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.AssignmentEntity", "Assignment")
                        .WithMany("Attempts")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeAssignment.Database.Entities.UserEntity", "User")
                        .WithMany("Attempts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.ChapterEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.CourseEntity", "Course")
                        .WithMany("Chapters")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Course");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.CourseEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.CategoryEntity", "Category")
                        .WithMany("Courses")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("HomeAssignment.Database.Entities.UserEntity", "User")
                        .WithMany("Courses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.EnrollmentEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.CourseEntity", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeAssignment.Database.Entities.UserEntity", "User")
                        .WithMany("Enrollments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.UserAssignmentProgressEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.AssignmentEntity", "Assignment")
                        .WithMany("UsersProgress")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeAssignment.Database.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.UserChapterProgressEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.ChapterEntity", "Chapter")
                        .WithMany("UsersProgress")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HomeAssignment.Database.Entities.UserEntity", "User")
                        .WithMany("UsersProgress")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chapter");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.UserRolesEntity", b =>
                {
                    b.HasOne("HomeAssignment.Database.Entities.RoleEntity", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HomeAssignment.Database.Entities.UserEntity", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.AssignmentEntity", b =>
                {
                    b.Navigation("Attempts");

                    b.Navigation("UsersProgress");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.CategoryEntity", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.ChapterEntity", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Attachments");

                    b.Navigation("UsersProgress");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.CourseEntity", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Chapters");

                    b.Navigation("Enrollments");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.RoleEntity", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("HomeAssignment.Database.Entities.UserEntity", b =>
                {
                    b.Navigation("Attempts");

                    b.Navigation("Courses");

                    b.Navigation("Enrollments");

                    b.Navigation("UserRoles");

                    b.Navigation("UsersProgress");
                });
#pragma warning restore 612, 618
        }
    }
}
