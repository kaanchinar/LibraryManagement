using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Genres.Queries.GetGenres;

public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, PagedResult<GenreDto>>
{
    private readonly IAppDbContext _context;

    public GetGenresQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<GenreDto>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        var query = _context.Genres.AsNoTracking();

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

        return new PagedResult<GenreDto>
        {
            Items = items.Select(g => g.ToDto()).ToList(),
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }
}
