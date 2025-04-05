using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.Persistence.Commands.AssignmentUserProgresses;
using HomeAssignment.Persistence.Queries.UserAssignmentProgresses;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.AssignmentRelated;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations.AssignmentRelated;

public class AssignmentProgressService(
    ILogger<AssignmentProgressService> logger,
    IDatabaseTransactionManager transactionManager,
    IMediator mediator,
    IMapper mapper)
    : BaseService<AssignmentProgressService>(logger, transactionManager), IAssignmentProgressService
{
    private readonly ILogger<AssignmentProgressService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    
    public async Task<RespondAssignmentUserProgressDto?> GetProgressByAssignmentIdAsync(Guid userId, Guid assignmentId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving progress for assignment {AssignmentId}", assignmentId);
        
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await mediator.Send(new GetAssignmentUserProgressByIdQuery(userId, assignmentId), cancellationToken));
        
        return mapper.Map<RespondAssignmentUserProgressDto>(result);
    }

    public async Task<RespondAssignmentUserProgressDto> UpdateProgressAsync(Guid userId, Guid assignmentId, bool completed, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving progress with {ASSIGNMENT_ID}", assignmentId);
        
        var userProgress = await ExecuteWithExceptionHandlingAsync(
            async () => await mediator.Send(new GetAssignmentUserProgressByIdQuery(userId, assignmentId), cancellationToken)
        );

        return userProgress == null
            ? await CreateUserProgressAsync(userId, assignmentId, cancellationToken)
            : await UpdateUserProgressAsync(userProgress, completed, cancellationToken);
    }
    
    private async Task<RespondAssignmentUserProgressDto> CreateUserProgressAsync(Guid userId, Guid assignmentId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started creating user progress with ASSIGNMENT_ID: {ASSIGNMENT_ID}", assignmentId);
        
        var progress = AssignmentUserProgress.Create(true, userId, assignmentId);
        var createdUserProgress = await ExecuteTransactionAsync(
            async () => await mediator.Send(new CreateAssignmentUserProgressCommand(progress), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully created user progress with ASSIGNMENT_ID: {ASSIGNMENT_ID}", assignmentId);
        return mapper.Map<RespondAssignmentUserProgressDto>(createdUserProgress);
    }

    private async Task<RespondAssignmentUserProgressDto> UpdateUserProgressAsync(AssignmentUserProgress userProgress, bool completed, CancellationToken cancellationToken)
    {
        userProgress.Update(completed);
        
        var updatedUserProgress = await ExecuteTransactionAsync(
            async () => await mediator.Send(new UpdateAssignmentUserProgressCommand(userProgress), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully updated user progress with ASSIGNMENT_ID: {ASSIGNMENT_ID}", userProgress.AssignmentId);
        return mapper.Map<RespondAssignmentUserProgressDto>(updatedUserProgress);
    }
}