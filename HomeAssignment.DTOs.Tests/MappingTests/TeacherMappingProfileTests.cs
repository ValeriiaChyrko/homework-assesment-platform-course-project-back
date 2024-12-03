using AutoMapper;
using FluentAssertions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.Tests.MappingTests;

public class TeacherMappingProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<TeacherMappingProfile>(); });
        _mapper = config.CreateMapper();
    }

    [Test]
    public void Teacher_To_UserDto_Should_Map_Correctly()
    {
        // Arrange
        var teacher = Teacher.Create
        (
            "John Doe",
            "john.doe@example.com",
            "hashed_password",
            "johndoe",
            "https://github.com/johndoe",
            "https://github.com/johndoe.png"
        );

        // Act
        var result = _mapper.Map<UserDto>(teacher);

        // Assert
        result.Id.Should().Be(teacher.UserId);
        result.FullName.Should().Be(teacher.FullName);
        result.Email.Should().Be(teacher.Email);
        result.PasswordHash.Should().Be(teacher.PasswordHash);
        result.RoleType.Should().Be(teacher.RoleType);
        result.CreatedAt.Should().Be(teacher.CreatedAt);
        result.UpdatedAt.Should().Be(teacher.UpdatedAt);
    }

    [Test]
    public void Teacher_To_RespondTeacherDto_Should_Map_Correctly()
    {
        // Arrange
        var teacher = Teacher.Create
        (
            "John Doe",
            "john.doe@example.com",
            "hashed_password",
            "johndoe",
            "https://github.com/johndoe",
            "https://github.com/johndoe.png"
        );

        // Act
        var result = _mapper.Map<RespondTeacherDto>(teacher);

        // Assert
        result.UserId.Should().Be(teacher.UserId);
        result.FullName.Should().Be(teacher.FullName);
        result.Email.Should().Be(teacher.Email);
        result.Password.Should().Be(teacher.PasswordHash);
        result.CreatedAt.Should().Be(teacher.CreatedAt);
        result.UpdatedAt.Should().Be(teacher.UpdatedAt);
    }

    [Test]
    public void UserDto_To_RespondTeacherDto_Should_Map_Correctly()
    {
        // Arrange
        var userDto = new UserDto
        {
            Id = Guid.NewGuid(),
            FullName = "Mark Smith",
            Email = "mark.smith@example.com",
            PasswordHash = "hashed_password",
            RoleType = "teacher",
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<RespondTeacherDto>(userDto);

        // Assert
        result.UserId.Should().Be(userDto.Id);
        result.FullName.Should().Be(userDto.FullName);
        result.Email.Should().Be(userDto.Email);
        result.Password.Should().Be(userDto.PasswordHash);
        result.CreatedAt.Should().Be(userDto.CreatedAt);
        result.UpdatedAt.Should().Be(userDto.UpdatedAt);
    }

    [Test]
    public void Teacher_To_GitHubProfileDto_Should_Map_Correctly()
    {
        // Arrange
        var teacher = Teacher.Create
        (
            "John Doe",
            "john.doe@example.com",
            "hashed_password",
            "johndoe",
            "https://github.com/johndoe",
            "https://github.com/johndoe.png"
        );

        // Act
        var result = _mapper.Map<GitHubProfileDto>(teacher);

        // Assert
        result.Id.Should().Be(teacher.GitHubProfileId);
        result.GithubUsername.Should().Be(teacher.GithubUsername);
        result.GithubProfileUrl.Should().Be(teacher.GithubProfileUrl);
        result.GithubPictureUrl.Should().Be(teacher.GithubPictureUrl);
    }
}