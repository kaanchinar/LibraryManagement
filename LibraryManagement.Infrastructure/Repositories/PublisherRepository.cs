using LibraryManagement.Application.Common;
using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class PublisherRepository(AppDbContext context) : IPublisherRepository
{
    public async Task<Publisher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Publishers.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Publisher?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Publishers
            .Include(p => p.Books)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Publisher>> GetPagedAsync(PublisherFilter filter, CancellationToken cancellationToken = default)
    {
        var query = context.Publishers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(p => p.Name.Contains(filter.Name));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "name" => filter.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            _ => filter.SortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt)
        };

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Publisher>
        {
            Items = items,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(Publisher publisher, CancellationToken cancellationToken = default)
    {
        await context.Publishers.AddAsync(publisher, cancellationToken);
    }

    public void Remove(Publisher publisher)
    {
        context.Publishers.Remove(publisher);
    }
}
