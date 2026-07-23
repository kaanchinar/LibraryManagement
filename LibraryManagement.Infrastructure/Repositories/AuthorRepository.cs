using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class AuthorRepository(AppDbContext context) : IAuthorRepository
{
    public async Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Authors.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Author?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Author>> GetPagedAsync(AuthorFilter filter, CancellationToken cancellationToken = default)
    {
        var query = context.Authors.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(a => a.Name.Contains(filter.Name));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "name" => filter.SortDescending ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
            _ => filter.SortDescending ? query.OrderByDescending(a => a.CreatedAt) : query.OrderBy(a => a.CreatedAt)
        };

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Author>
        {
            Items = items,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(Author author, CancellationToken cancellationToken = default)
    {
        await context.Authors.AddAsync(author, cancellationToken);
    }

    public void Remove(Author author)
    {
        context.Authors.Remove(author);
    }
}
