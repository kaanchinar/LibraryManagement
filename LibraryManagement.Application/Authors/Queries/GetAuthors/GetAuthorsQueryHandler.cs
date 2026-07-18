using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Authors.Queries.GetAuthors;

public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, PagedResult<AuthorDto>>
{
    private readonly IAppDbContext _context;

    public GetAuthorsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        var query = _context.Authors.AsNoTracking();

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

        return new PagedResult<AuthorDto>
        {
            Items = items.Select(a => a.ToDto()).ToList(),
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }
}
