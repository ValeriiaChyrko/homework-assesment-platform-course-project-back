﻿using HomeAssignment.Database.Contexts.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Commands.ChapterUserProgresses;

public sealed class DeleteCourseUserProgressCommandHandler(IHomeworkAssignmentDbContext context)
    : IRequestHandler<DeleteCourseUserProgressCommand>
{
    private readonly IHomeworkAssignmentDbContext
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task Handle(DeleteCourseUserProgressCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var chapters = await _context.ChapterEntities
            .Where(ch => ch.CourseId == command.CourseId)
            .Select(ch => ch.Id)
            .ToListAsync(cancellationToken);

        if (chapters.Count == 0)
            return;

        await _context.UserChapterProgressEntities
            .Where(up => up.ChapterId != null && chapters.Contains(up.ChapterId.Value))
            .ExecuteDeleteAsync(cancellationToken);
    }
}