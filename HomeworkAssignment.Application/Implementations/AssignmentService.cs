using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.Assignments;
using HomeAssignment.Persistence.Queries.Assignments;
using HomeAssignment.Persistence.Queries.Users;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Implementations;

public class AssignmentService : BaseService<AssignmentService>, IAssignmentService
{
    private readonly IChapterService _chapterService;
    
    private readonly ILogger<AssignmentService> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AssignmentService(IChapterService chapterService, IDatabaseTransactionManager transactionManager, IMediator mediator,
        ILogger<AssignmentService> logger, IMapper mapper)
        : base(logger, transactionManager)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper;
        _chapterService = chapterService;
    }

    public async Task<RespondAssignmentDto> CreateAssignmentAsync(
        Guid userId,
        Guid chapterId,
        RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation("Started creating assignment: {@AssignmentDto}", assignmentDto);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var lastAssignment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetLastAssignmentByIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        var newPosition = lastAssignment != null ? lastAssignment.Position + 1 : 1;
        
        var assignment = _mapper.Map<Assignment>(assignmentDto);
        assignment.Position = newPosition;
        assignment.ChapterId = chapterId;
        
        assignment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CreateAssignmentCommand(assignment), cancellationToken),
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Successfully created assignment with ID: {AssignmentId}", assignment.Id);
        return _mapper.Map<RespondAssignmentDto>(assignment);
    }

    public async Task<RespondAssignmentDto> UpdateAssignmentAsync(Guid userId, Guid chapterId, Guid assignmentId, RequestAssignmentDto assignmentDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started updating assignment with ID: {AssignmentId}", assignmentId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var assignment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetAssignmentByIdQuery(assignmentId, chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        if (assignment == null)
        {
            _logger.LogWarning("Assignment with ID: {id} does not exist.", assignmentId);
            throw new ArgumentException("Assignment does not exist.");
        }
        assignment.Update(
            assignmentDto.Title,
            assignmentDto.Description,
            assignmentDto.RepositoryName,
            assignmentDto.RepositoryOwner,
            assignmentDto.RepositoryUrl,
            assignmentDto.Deadline,
            assignmentDto.MaxScore,
            assignmentDto.MaxAttemptsAmount,
            assignmentDto.Position,
            new ScoreSection(assignmentDto.AttemptCompilationSectionEnable, assignmentDto.AttemptCompilationMaxScore, assignmentDto.AttemptCompilationMinScore),
            new ScoreSection(assignmentDto.AttemptTestsSectionEnable, assignmentDto.AttemptTestsMaxScore, assignmentDto.AttemptTestsMinScore),
            new ScoreSection(assignmentDto.AttemptQualitySectionEnable, assignmentDto.AttemptQualityMaxScore, assignmentDto.AttemptQualityMinScore)
        );
        
        var updatedAssignment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateAssignmentCommand(assignmentId, assignment), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully updated assignment: {@Assignment}", updatedAssignment);
        return _mapper.Map<RespondAssignmentDto>(updatedAssignment);
    }

    public async Task DeleteAssignmentAsync(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started deleting assignment with ID: {AssignmentId}", assignmentId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        await ExecuteTransactionAsync(
            async () => await _mediator.Send(new DeleteAssignmentCommand(assignmentId), cancellationToken),
            cancellationToken: cancellationToken);
        
        var isAnyPublishedAssignmentInChapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new IsAnyPublishedAssignmentByChapterIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isAnyPublishedAssignmentInChapter)
        {
            await _chapterService.UnpublishChapterAsync(userId, courseId, chapterId, cancellationToken);
        }
        
        _logger.LogInformation("Successfully deleted assignment with ID: {AssignmentId}", assignmentId);
    }
    
    public async Task<RespondAssignmentDto> PublishAssignmentAsync(Guid userId, Guid chapterId, Guid assignmentId, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started publish assignment with ID: {AssignmentId}", assignmentId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var assignment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetAssignmentByIdQuery(assignmentId, chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        if (assignment == null)
        {
            _logger.LogWarning("Assignment with ID: {id} does not exist.", assignmentId);
            throw new ArgumentException("Assignment does not exist.");
        }
        assignment.Publish();
        
        var updatedAssignment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateAssignmentCommand(assignmentId, assignment), cancellationToken),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully published assignment: {@Assignment}", updatedAssignment);
        return _mapper.Map<RespondAssignmentDto>(updatedAssignment);
    }
    
    public async Task<RespondAssignmentDto> UnpublishAssignmentAsync(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started unpublish assignment with ID: {AssignmentId}", assignmentId);
        var isTeacher = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new CheckIfUserInRoleQuery(userId, UserRoles.Teacher), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isTeacher)
        {
            _logger.LogWarning("User with ID: {userId} does not have an teacher role.", userId);
            throw new ArgumentException("User does not have an teacher role.");
        }
        
        var assignment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new GetAssignmentByIdQuery(assignmentId, chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        if (assignment == null)
        {
            _logger.LogWarning("Assignment with ID: {id} does not exist.", assignmentId);
            throw new ArgumentException("Assignment does not exist.");
        }
        assignment.Unpublish();
        
        var updatedAssignment = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new UpdateAssignmentCommand(assignmentId, assignment), cancellationToken),
            cancellationToken: cancellationToken);
        
        var isAnyPublishedAssignmentInChapter = await ExecuteTransactionAsync(
            async () => await _mediator.Send(new IsAnyPublishedAssignmentByChapterIdQuery(chapterId), cancellationToken),
            cancellationToken: cancellationToken);
        if (!isAnyPublishedAssignmentInChapter)
        {
            await _chapterService.UnpublishChapterAsync(userId, courseId, chapterId, cancellationToken);
        }
        
        _logger.LogInformation("Successfully unpublished assignment: {@Assignment}", updatedAssignment);
        return _mapper.Map<RespondAssignmentDto>(updatedAssignment);
    }

    public async Task<PagedList<RespondAssignmentDto>> GetAssignmentsAsync(
        RequestAssignmentFilterParameters filterParameters,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving all assignments");

        var query = new GetAllAssignmentsQuery(filterParameters);
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(query, cancellationToken)
        );

        _logger.LogInformation("Successfully retrieved all assignments");
        
        var mappedItems = result.Items.Select(entityModel => _mapper.Map<RespondAssignmentDto>(entityModel)).ToList();
        var pagedResult = new PagedList<RespondAssignmentDto>(mappedItems, result.TotalCount, result.Page, result.PageSize);

        return pagedResult;
    }
    
    public async Task<RespondAssignmentDto?> GetAssignmentByIdAsync(Guid chapterId, Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started retrieving assignment");
        
        var result = await ExecuteWithExceptionHandlingAsync(
            async () => await _mediator.Send(GetAssignmentByIdAsync(chapterId, assignmentId), cancellationToken)
        );

        if (result == null)
        {
            _logger.LogWarning("Assignment with ID: {id} does not exist.", assignmentId);
            return null;
        }

        _logger.LogInformation("Successfully retrieved assignment");
        return _mapper.Map<RespondAssignmentDto>(result);
    }
}