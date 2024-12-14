using FluentAssertions;
using HomeAssignment.Database.Contexts.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeAssignment.Database.Tests;

[TestFixture]
public class DatabaseDbContextTests
{
    [Test]
    public void CreateDbContext_ReturnsDbContextInstance()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var factory = new HomeworkAssignmentPostgresDbContextFactory(configuration);

        // Act
        var dbContext = factory.CreateDbContext();

        // Assert
        dbContext.Should().NotBeNull();
        dbContext.Should().BeAssignableTo<HomeworkAssignmentDbContext>();
    }
    [Test]
    public void CreateDbContext_ConfigurationIsLoadedCorrectly()
    {
        // Arrange
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../HomeworkAssignment.API");
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.Development.json", false, true)
            .Build();
        var factory = new HomeworkAssignmentPostgresDbContextFactory(config);

        // Act
        var dbContext = factory.CreateDbContext();

        // Assert
        dbContext.Database.GetDbConnection().ConnectionString.Should()
            .Be(config.GetConnectionString("DefaultConnection"));
    }
    [Test]
    public void CreateDbContext_OptionsBuilderIsConfiguredCorrectly()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var factory = new HomeworkAssignmentPostgresDbContextFactory(configuration);

        // Act
        var dbContext = factory.CreateDbContext();

        // Assert
        dbContext.Database.ProviderName.Should().Be("Npgsql.EntityFrameworkCore.PostgreSQL");
    }
}