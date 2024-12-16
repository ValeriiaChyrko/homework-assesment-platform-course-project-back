using FluentAssertions;
using HomeworkAssignment.Application.Implementations;

namespace HomeworkAssignment.Application.Tests;

[TestFixture]
public class PasswordHasherTests
{
    private PasswordHasher _passwordHasher;

    [SetUp]
    public void SetUp()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Test]
    public void HashPassword_ShouldReturnNonNullAndNonEmptyHash()
    {
        // Arrange
        const string password = "StrongPassword123!";

        // Act
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Assert
        hashedPassword.Should().NotBeNullOrWhiteSpace();
    }
    [Test]
    public void VerifyHashedPassword_ShouldReturnTrue_WhenPasswordMatchesHash()
    {
        // Arrange
        const string password = "StrongPassword123!";
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyHashedPassword(hashedPassword, password);

        // Assert
        result.Should().BeTrue();
    }
    [Test]
    public void VerifyHashedPassword_ShouldReturnFalse_WhenPasswordDoesNotMatchHash()
    {
        // Arrange
        const string password = "StrongPassword123!";
        var hashedPassword = _passwordHasher.HashPassword(password);
        const string wrongPassword = "WrongPassword456!";

        // Act
        var result = _passwordHasher.VerifyHashedPassword(hashedPassword, wrongPassword);

        // Assert
        result.Should().BeFalse();
    }
    [Test]
    public void HashPassword_ShouldGenerateUniqueHashes_ForSamePassword()
    {
        // Arrange
        const string password = "RepeatablePassword!";

        // Act
        var hash1 = _passwordHasher.HashPassword(password);
        var hash2 = _passwordHasher.HashPassword(password);

        // Assert
        hash1.Should().NotBe(hash2);
    }
    [Test]
    public void VerifyHashedPassword_ShouldReturnFalse_WhenInvalidHashProvided()
    {
        // Arrange
        const string invalidHash = "InvalidHash";
        const string password = "SomePassword";

        // Act
        var result = _passwordHasher.VerifyHashedPassword(invalidHash, password);

        // Assert
        result.Should().BeFalse();
    }
}