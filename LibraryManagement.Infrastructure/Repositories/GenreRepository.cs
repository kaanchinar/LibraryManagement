using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class GenreRepository(AppDbContext context) : IGenreRepository
{
    public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Genres.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Genre?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Genres
            .Include(g => g.Books)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Genre>> GetPagedAsync(GenreFilter filter, CancellationToken cancellationToken = default)
    {
        var query = context.Genres.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(g => g.Name.Contains(filter.Name));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "name" => filter.SortDescending ? query.OrderByDescending(g => g.Name) : query.OrderBy(g => g.Name),
            _ => filter.SortDescending ? query.OrderByDescending(g => g.CreatedAt) : query.OrderBy(g => g.CreatedAt)
        };

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Genre>
        {
            Items = items,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        await context.Genres.AddAsync(genre, cancellationToken);
    }

    public void Remove(Genre genre)
    {
        context.Genres.Remove(genre);
    }
}
