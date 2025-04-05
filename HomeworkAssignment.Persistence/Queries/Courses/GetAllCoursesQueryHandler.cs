using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.Persistence.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses
{
    public sealed class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, PagedList<CourseDetailView>>
    {
        private readonly IHomeworkAssignmentDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCoursesQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedList<CourseDetailView>> Handle(GetAllCoursesQuery query, CancellationToken cancellationToken)
        {
            var coursesQuery = _context.CourseEntities.AsNoTracking();
            
            if (!string.IsNullOrEmpty(query.FilterParameters.Title))
            {
                coursesQuery = coursesQuery.Where(a => a.Title.Contains(query.FilterParameters.Title));
            }

            if (query.FilterParameters.CategoryId.HasValue)
            {
                coursesQuery = coursesQuery.Where(a => a.CategoryId == query.FilterParameters.CategoryId.Value);
            }
            
            if (query.FilterParameters.IsPublished)
            {
                coursesQuery = coursesQuery.Where(a => a.IsPublished == query.FilterParameters.IsPublished);
            }
            
            if (query.FilterParameters.IncludeCategory)
            {
                coursesQuery = coursesQuery.Include(a => a.Category);
            }
            
            if (query.FilterParameters.IncludeChapters)
            {
                coursesQuery = coursesQuery.Include(a => a.Chapters);
            }
            
            
            coursesQuery = coursesQuery.OrderByDescending(a => a.CreatedAt);

            var assignmentDtos = coursesQuery.Select(entityModel => _mapper.Map<CourseDetailView>(entityModel));
            return await PagedList<CourseDetailView>.CreateAsync(assignmentDtos, query.FilterParameters.Page, query.FilterParameters.PageSize, cancellationToken);
        }
    }
}