using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Entities;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.Tests.MappingTests;

public class MappingProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Test]
    public void UserDto_To_UserEntity_Should_Map_Correctly()
    {
        // Arrange
        var userDto = new UserDto
        {
            Id = Guid.NewGuid(),
            FullName = "John Doe",
            Email = "john.doe@example.com",
            PasswordHash = "hashed_password",
            RoleType = "Admin",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<UserEntity>(userDto);

        // Assert
        result.Id.Should().Be(userDto.Id);
        result.FullName.Should().Be(userDto.FullName);
        result.Email.Should().Be(userDto.Email);
        result.PasswordHash.Should().Be(userDto.PasswordHash);
        result.RoleType.Should().Be(userDto.RoleType);
        result.CreatedAt.Should().Be(userDto.CreatedAt);
        result.UpdatedAt.Should().Be(userDto.UpdatedAt);
    }

    [Test]
    public void UserEntity_To_UserDto_Should_Map_Correctly()
    {
        // Arrange
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            FullName = "Jane Smith",
            Email = "jane.smith@example.com",
            PasswordHash = "hashed_password_123",
            RoleType = "User",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<UserDto>(userEntity);

        // Assert
        result.Id.Should().Be(userEntity.Id);
        result.FullName.Should().Be(userEntity.FullName);
        result.Email.Should().Be(userEntity.Email);
        result.PasswordHash.Should().Be(userEntity.PasswordHash);
        result.RoleType.Should().Be(userEntity.RoleType);
        result.CreatedAt.Should().Be(userEntity.CreatedAt);
        result.UpdatedAt.Should().Be(userEntity.UpdatedAt);
    }

    [Test]
    public void GitHubProfileDto_To_GitHubProfilesEntity_Should_Map_Correctly()
    {
        // Arrange
        var gitHubProfileDto = new GitHubProfileDto
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            GithubUsername = "github_user",
            GithubProfileUrl = "https://github.com/github_user",
            GithubPictureUrl = "https://avatars.githubusercontent.com/github_user"
        };

        // Act
        var result = _mapper.Map<GitHubProfilesEntity>(gitHubProfileDto);

        // Assert
        result.Id.Should().Be(gitHubProfileDto.Id);
        result.UserId.Should().Be(gitHubProfileDto.UserId);
        result.GithubUsername.Should().Be(gitHubProfileDto.GithubUsername);
        result.GithubProfileUrl.Should().Be(gitHubProfileDto.GithubProfileUrl);
        result.GithubPictureUrl.Should().Be(gitHubProfileDto.GithubPictureUrl);
    }

    [Test]
    public void GitHubProfilesEntity_To_GitHubProfileDto_Should_Map_Correctly()
    {
        // Arrange
        var gitHubProfilesEntity = new GitHubProfilesEntity
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            GithubUsername = "another_user",
            GithubProfileUrl = "https://github.com/another_user",
            GithubPictureUrl = null
        };

        // Act
        var result = _mapper.Map<GitHubProfileDto>(gitHubProfilesEntity);

        // Assert
        result.Id.Should().Be(gitHubProfilesEntity.Id);
        result.UserId.Should().Be(gitHubProfilesEntity.UserId);
        result.GithubUsername.Should().Be(gitHubProfilesEntity.GithubUsername);
        result.GithubProfileUrl.Should().Be(gitHubProfilesEntity.GithubProfileUrl);
        result.GithubPictureUrl.Should().Be(gitHubProfilesEntity.GithubPictureUrl);
    }
}