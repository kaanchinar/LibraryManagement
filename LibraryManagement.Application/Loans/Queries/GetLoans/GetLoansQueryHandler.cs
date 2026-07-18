using LibraryManagement.Application.Common;
using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Loans.Queries.GetLoans;

public class GetLoansQueryHandler : IRequestHandler<GetLoansQuery, PagedResult<LoanDto>>
{
    private readonly IAppDbContext _context;

    public GetLoansQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<LoanDto>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        IQueryable<Loan> query = _context.Loans.AsNoTracking()
            .Include(l => l.Book)
            .Include(l => l.Member);

        if (filter.MemberId.HasValue)
        {
            query = query.Where(l => l.MemberId == filter.MemberId.Value);
        }

        if (filter.BookId.HasValue)
        {
            query = query.Where(l => l.BookId == filter.BookId.Value);
        }

        if (filter.IsReturned.HasValue)
        {
            query = query.Where(l => l.IsReturned == filter.IsReturned.Value);
        }

        if (filter.IsOverdue.HasValue)
        {
            var now = DateTime.UtcNow;
            query = filter.IsOverdue.Value
                ? query.Where(l => !l.IsReturned && l.DueDate < now)
                : query.Where(l => l.IsReturned || l.DueDate >= now);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "loandate" => filter.SortDescending ? query.OrderByDescending(l => l.LoanDate) : query.OrderBy(l => l.LoanDate),
            "duedate" => filter.SortDescending ? query.OrderByDescending(l => l.DueDate) : query.OrderBy(l => l.DueDate),
            _ => filter.SortDescending ? query.OrderByDescending(l => l.CreatedAt) : query.OrderBy(l => l.CreatedAt)
        };

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<LoanDto>
        {
            Items = items.Select(l => l.ToDto()).ToList(),
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }
}
