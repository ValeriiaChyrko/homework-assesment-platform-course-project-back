using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.Persistence.Abstractions;

public class PagedList<T>
{
    public PagedList(List<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public List<T> Items { get; private set; }
    public int TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page * PageSize < TotalCount;

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedList<T>(items, totalCount, page, pageSize);
    }
}