using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;
using HomeworkAssignment.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers;

[Produces("application/json")]
[ApiController]
public class ChapterController : ControllerBase
{
    private readonly IChapterService _chapterService;

    public ChapterController(IChapterService chapterService)
    {
        _chapterService = chapterService;
    }

    [HttpGet("/api/courses/${courseId:guid}/chapters")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondChapterDto>> Get(
        [FromBody] RequestChapterFilterParameters filterParameters,
        [FromServices] IValidator<RequestChapterFilterParameters> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(filterParameters, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _chapterService.GetChaptersAsync(filterParameters, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpGet("/api/courses/${courseId:guid}/chapters/${chapterId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondChapterDto>> Get(
        Guid chapterId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _chapterService.GetChapterByIdAsync(chapterId, cancellationToken);
        if (result == null) return StatusCode(StatusCodes.Status404NotFound);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost("/api/courses/${courseId:guid}/chapters")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondChapterDto>> Create(
        Guid userId, 
        Guid chapterId,
        [FromBody] RequestChapterDto request,
        [FromServices] IValidator<RequestChapterDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _chapterService.CreateChapterAsync(userId, chapterId, request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("/api/courses/${courseId:guid}/chapters/${chapterId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(
        Guid userId, 
        Guid courseId, 
        Guid chapterId, 
        CancellationToken cancellationToken = default
    )
    {
        await _chapterService.DeleteChapterAsync(userId, courseId, chapterId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, chapterId);
    }

    [HttpPatch("/api/courses/${courseId:guid}/chapters/${chapterId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RespondChapterDto>> Update(
        Guid userId, 
        Guid courseId,
        Guid chapterId, 
        [FromBody] RequestChapterDto request,
        [FromServices] IValidator<RequestChapterDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var response = await _chapterService.UpdateChapterAsync(userId, courseId, chapterId, request, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, response);
    }
    
    [HttpPut("/api/courses/${courseId}/chapters/${chapterId}/progress")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> UpdateProgress(
        Guid userId, 
        Guid courseId,  
        Guid chapterId,
        [FromBody] RequestUserProgressDto request,
        [FromServices] IValidator<RequestUserProgressDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);
        
        await _chapterService.UpdateProgressAsync(userId, courseId, chapterId, request, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, courseId);
    }
    
    [HttpPatch("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/publish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Publish(
        Guid userId,
        Guid courseId,
        Guid chapterId, 
        CancellationToken cancellationToken = default
    )
    {
        await _chapterService.PublishChapterAsync(userId, courseId, chapterId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, chapterId);
    }
    
    [HttpPatch("/api/courses/${courseId:guid}/chapters/${chapterId:guid}/unpublish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Unpublish(
        Guid userId, 
        Guid courseId, 
        Guid chapterId, 
        CancellationToken cancellationToken = default
    )
    {
        await _chapterService.UnpublishChapterAsync(userId, courseId, chapterId, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, chapterId);
    }
    
    [HttpPut("/api/courses/${courseId:guid}/chapters/reorder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Reorder(
        Guid userId, 
        Guid courseId,  
        [FromBody] IEnumerable<RespondChapterDto> assignmentDtos,
        CancellationToken cancellationToken = default
    )
    {
        await _chapterService.ReorderChapterAsync(userId, courseId, assignmentDtos, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, courseId);
    }
}