using Microsoft.EntityFrameworkCore;

namespace HomeAssignment.DTOs.SharedDTOs;

public class PagedList<T>
{
    public List<T> Items { get; private set; }
    public int TotalCount { get; private set; }
    public int Page { get; private set; }
    public int PageSize { get; private set; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page * PageSize < TotalCount;

    public PagedList(List<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken: cancellationToken);

        return new PagedList<T>(items, totalCount, page, pageSize);
    }
}