using LibraryManagement.Application.Common;
using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Members.Queries.GetMembers;

public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, PagedResult<MemberDto>>
{
    private readonly IAppDbContext _context;

    public GetMembersQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        var query = _context.Members.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.FullName))
        {
            query = query.Where(m => m.FullName.Contains(filter.FullName));
        }

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(m => m.Email.Contains(filter.Email));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(m => m.IsActive == filter.IsActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "fullname" => filter.SortDescending ? query.OrderByDescending(m => m.FullName) : query.OrderBy(m => m.FullName),
            "email" => filter.SortDescending ? query.OrderByDescending(m => m.Email) : query.OrderBy(m => m.Email),
            _ => filter.SortDescending ? query.OrderByDescending(m => m.CreatedAt) : query.OrderBy(m => m.CreatedAt)
        };

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<MemberDto>
        {
            Items = items.Select(m => m.ToDto()).ToList(),
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }
}
