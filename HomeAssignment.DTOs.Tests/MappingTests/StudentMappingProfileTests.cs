using AutoMapper;
using FluentAssertions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.Tests.MappingTests;

[TestFixture]
public class StudentMappingProfileTests
{
    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<StudentMappingProfile>(); });
        _mapper = config.CreateMapper();
    }

    private IMapper _mapper;

    [Test]
    public void Student_To_RespondStudentDto_Should_Map_Correctly()
    {
        // Arrange
        var student = Student.Create(
            "John Doe",
            "john.doe@example.com",
            "hashed_password",
            "johndoe",
            "https://github.com/johndoe",
            "https://github.com/johndoe/picture.jpg"
        );

        // Act
        var result = _mapper.Map<RespondStudentDto>(student);

        // Assert
        result.UserId.Should().Be(student.UserId);
        result.FullName.Should().Be(student.FullName);
        result.Email.Should().Be(student.Email);
        result.Password.Should().Be(student.PasswordHash);
    }

    [Test]
    public void Student_To_UserDto_Should_Map_Correctly()
    {
        // Arrange
        var student = Student.Create(
            "John Doe",
            "john.doe@example.com",
            "hashed_password",
            "johndoe",
            "https://github.com/johndoe",
            "https://github.com/johndoe/picture.jpg"
        );

        // Act
        var result = _mapper.Map<UserDto>(student);

        // Assert
        result.Id.Should().Be(student.UserId);
        result.FullName.Should().Be(student.FullName);
        result.Email.Should().Be(student.Email);
    }

    [Test]
    public void Student_To_GitHubProfileDto_Should_Map_Correctly()
    {
        // Arrange
        var student = Student.Create(
            "John Doe",
            "john.doe@example.com",
            "hashed_password",
            "johndoe",
            "https://github.com/johndoe",
            "https://github.com/johndoe/picture.jpg"
        );

        // Act
        var result = _mapper.Map<GitHubProfileDto>(student);

        // Assert
        result.Id.Should().Be(student.GitHubProfileId);
        result.GithubUsername.Should().Be(student.GithubUsername);
        result.GithubProfileUrl.Should().Be(student.GithubProfileUrl);
        result.GithubPictureUrl.Should().Be(student.GithubPictureUrl);
    }

    [Test]
    public void UserDto_To_RespondStudentDto_Should_Map_Correctly()
    {
        // Arrange
        var userDto = new UserDto
        {
            Id = Guid.NewGuid(),
            FullName = "Jane Doe",
            Email = "jane.doe@example.com",
            PasswordHash = "hashed_password",
            RoleType = "student",
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<RespondStudentDto>(userDto);

        // Assert
        result.UserId.Should().Be(userDto.Id);
        result.FullName.Should().Be(userDto.FullName);
        result.Email.Should().Be(userDto.Email);
        result.Password.Should().Be(userDto.PasswordHash);
        result.CreatedAt.Should().Be(userDto.CreatedAt);
        result.UpdatedAt.Should().Be(userDto.UpdatedAt);
    }
}