using LibraryManagement.Application.Common;
using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Publishers.Queries.GetPublishers;

public class GetPublishersQueryHandler : IRequestHandler<GetPublishersQuery, PagedResult<PublisherDto>>
{
    private readonly IAppDbContext _context;

    public GetPublishersQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<PublisherDto>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        var query = _context.Publishers.AsNoTracking();

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

        return new PagedResult<PublisherDto>
        {
            Items = items.Select(p => p.ToDto()).ToList(),
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }
}
