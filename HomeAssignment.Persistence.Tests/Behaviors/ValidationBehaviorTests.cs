using FluentValidation;
using FluentValidation.Results;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using HomeAssignment.Persistence.Behaviors;
using MediatR;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.Behaviors;

[TestFixture]
public class ValidationBehaviorTests
{
    [SetUp]
    public void SetUp()
    {
        _next = Substitute.For<RequestHandlerDelegate<TestResponse>>();
    }

    private ValidationBehavior<TestRequest, TestResponse> _behavior = null!;
    private IEnumerable<IValidator<TestRequest>> _validators = null!;
    private RequestHandlerDelegate<TestResponse> _next = null!;

    [Test]
    public async Task Handle_NoValidatorsProvided_ShouldCallNextDelegate()
    {
        // Arrange
        _validators = Enumerable.Empty<IValidator<TestRequest>>();
        _behavior = new ValidationBehavior<TestRequest, TestResponse>(_validators);

        // Act
        await _behavior.Handle(new TestRequest(), _next, CancellationToken.None);

        // Assert
        await _next.Received(1).Invoke();
    }

    [Test]
    public async Task Handle_ValidatorsProvided_NoValidationErrors_ShouldCallNextDelegate()
    {
        // Arrange
        var validator = Substitute.For<IValidator<TestRequest>>();
        validator.Validate(Arg.Any<ValidationContext<TestRequest>>()).Returns(new ValidationResult());
        _validators = new List<IValidator<TestRequest>> { validator };
        _behavior = new ValidationBehavior<TestRequest, TestResponse>(_validators);

        // Act
        await _behavior.Handle(new TestRequest(), _next, CancellationToken.None);

        // Assert
        await _next.Received(1).Invoke();
    }

    [Test]
    public async Task Handle_ValidatorsProvided_ValidationErrors_ShouldThrowRequestValidationException()
    {
        // Arrange
        var validator = Substitute.For<IValidator<TestRequest>>();
        var validationFailure = new ValidationFailure("TestField", "Test error message");
        var validationResult = new ValidationResult(
            new List<ValidationFailure> { validationFailure });

        validator.Validate(Arg.Any<ValidationContext<TestRequest>>()).Returns(validationResult);
        _validators = new List<IValidator<TestRequest>> { validator };
        _behavior = new ValidationBehavior<TestRequest, TestResponse>(_validators);

        // Act and Assert
        Assert.ThrowsAsync<RequestValidationException>(async () =>
        {
            await _behavior.Handle(new TestRequest(), _next, CancellationToken.None);
        });

        await _next.DidNotReceive().Invoke();
    }

    public class TestRequest : IRequest<TestResponse>
    {
    }

    public class TestResponse
    {
    }
}