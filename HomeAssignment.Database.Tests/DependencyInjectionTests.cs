using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Contexts.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace HomeAssignment.Database.Tests;

[TestFixture]
public class DependencyInjectionTests
{
    [Test]
    public void AddDatabaseServices_RegistersExpectedServices()
    {
        // Arrange
        var services = new ServiceCollection();
        Substitute.For<IConfiguration>();

        // Act
        services.AddDatabaseServices();

        // Assert
        services.Should().ContainSingle(descriptor =>
            descriptor.ServiceType == typeof(IHomeworkAssignmentDbContextProvider) &&
            descriptor.Lifetime == ServiceLifetime.Scoped);

        services.Should().ContainSingle(descriptor =>
            descriptor.ServiceType == typeof(IHomeworkAssignmentDbContextFactory) &&
            descriptor.Lifetime == ServiceLifetime.Scoped);

        services.Should().ContainSingle(descriptor =>
            descriptor.ServiceType == typeof(IHomeworkAssignmentDbContext) &&
            descriptor.Lifetime == ServiceLifetime.Scoped);
    }
    [Test]
    public void GetFactory_ReturnsCorrectFactory_ForPostgresSqlDbContext()
    {
        // Arrange
        var configMock = Substitute.For<IConfiguration>();
        configMock["DbContext:Type"].Returns("PostgresSql");

        var services = new ServiceCollection();
        services.AddSingleton(configMock);
        services.AddDatabaseServices();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var factory = serviceProvider.GetRequiredService<IHomeworkAssignmentDbContextFactory>();

        // Assert
        factory.Should().NotBeNull();
        factory.Should().BeOfType<HomeworkAssignmentPostgresDbContextFactory>();
    }
    [Test]
    public void GetFactory_ThrowsException_ForInvalidDbContextType()
    {
        // Arrange
        var configMock = Substitute.For<IConfiguration>();
        configMock["DbContext:Type"].Returns("InvalidType");
        
        var services = new ServiceCollection();
        services.AddSingleton(configMock);
        services.AddDatabaseServices();

        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        Action act = () => serviceProvider.GetRequiredService<IHomeworkAssignmentDbContextFactory>();
        act.Should().Throw<ArgumentException>();
    }
    [Test]
    public void AddDatabaseServices_RegistersCorrectDbContextInstance()
    {
        // Arrange
        var configMock = Substitute.For<IConfiguration>();
        configMock["DbContext:Type"].Returns("PostgresSql"); 

        var services = new ServiceCollection();
        services.AddSingleton(configMock);
        services.AddDatabaseServices();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var dbContext = serviceProvider.GetRequiredService<IHomeworkAssignmentDbContext>();

        // Assert
        dbContext.Should().NotBeNull();
        dbContext.Should().BeOfType<HomeworkAssignmentDbContext>();
    }
}