using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Categories;

public sealed class GetCategoryByCourseIdQueryHandler : IRequestHandler<GetCategoryByCourseIdQuery, Category?>
{
    private readonly IHomeworkAssignmentDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryByCourseIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Category?> Handle(GetCategoryByCourseIdQuery query, CancellationToken cancellationToken)
    {
        var categoryEntity = await _context
            .CategoryEntities
            .AsNoTracking()
            .Include(c => c.Courses) 
            .SingleOrDefaultAsync(c => 
                    c.Courses.Any(course => course.Id == query.CourseId), 
                cancellationToken
            );

        return categoryEntity != null ? _mapper.Map<Category>(categoryEntity) : null;
    }
}