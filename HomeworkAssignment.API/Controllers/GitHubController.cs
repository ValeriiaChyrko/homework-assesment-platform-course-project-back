using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace HomeworkAssignment.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
[Route("api/github")]
[Produces("application/json")]
[ApiController]
public class GitHubController : ControllerBase
{
    private readonly IAccountGrpcService _accountGrpc;
    private readonly ICompilationGrpcService _compilationGrpc;
    private readonly IQualityGrpcService _qualityGrpc;
    private readonly ITestsGrpcService _testsGrpc;

    public GitHubController(IAccountGrpcService accountGrpc, ICompilationGrpcService compilationGrpc,
        IQualityGrpcService qualityGrpc, ITestsGrpcService testsGrpc)
    {
        _accountGrpc = accountGrpc;
        _compilationGrpc = compilationGrpc;
        _qualityGrpc = qualityGrpc;
        _testsGrpc = testsGrpc;
    }

    [HttpGet("account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<string>>> GetAuthorBranches(
        [FromQuery] RequestBranchDto query,
        [FromServices] IValidator<RequestBranchDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _accountGrpc.GetBranchesAsync(query, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("compilation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetProjectCompilationVerificationScore(
        [FromQuery] RequestRepositoryWithBranchDto query,
        [FromServices] IValidator<RequestRepositoryWithBranchDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _compilationGrpc.VerifyProjectCompilation(query, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("quality")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetProjectQualityVerificationScore(
        [FromQuery] RequestRepositoryWithBranchDto query,
        [FromServices] IValidator<RequestRepositoryWithBranchDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _qualityGrpc.VerifyProjectQualityAsync(query, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetProjectTestsVerificationScore(
        [FromQuery] RequestRepositoryWithBranchDto query,
        [FromServices] IValidator<RequestRepositoryWithBranchDto> validator,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid) return StatusCode(StatusCodes.Status400BadRequest, validationResult.Errors);

        var result = await _testsGrpc.VerifyProjectPassedTestsAsync(query, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, result);
    }
}