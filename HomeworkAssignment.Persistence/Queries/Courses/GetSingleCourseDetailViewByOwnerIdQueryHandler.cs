using AutoMapper;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Queries.Courses
{
    public sealed class GetSingleCourseDetailViewByOwnerIdQueryHandler : IRequestHandler<GetSingleCourseDetailViewByOwnerIdQuery, CourseDetailView?>
    {
        private readonly IHomeworkAssignmentDbContext _context;
        private readonly IMapper _mapper;

        public GetSingleCourseDetailViewByOwnerIdQueryHandler(IHomeworkAssignmentDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CourseDetailView?> Handle(GetSingleCourseDetailViewByOwnerIdQuery query, CancellationToken cancellationToken)
        {
            var queryable = _context.CourseEntities
                .Where(a => a.UserId == query.UserId && a.Id == query.CourseId)
                .AsNoTracking();

            if (query.FilterParameters.IncludeCategory)
                queryable = queryable.Include(a => a.Category);

            if (query.FilterParameters.IncludeChapters)
                queryable = queryable.Include(a => a.Chapters);

            if (query.FilterParameters.IncludeAttachments)
                queryable = queryable.Include(a => a.Attachments);

            var courseEntity = await queryable.SingleOrDefaultAsync(cancellationToken);
            return courseEntity != null ? _mapper.Map<CourseDetailView>(courseEntity) : null;
        }
    }
}