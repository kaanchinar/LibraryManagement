using LibraryManagement.Application.Common;
using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Members.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Member?> GetByIdWithLoansAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Members
            .Include(m => m.Loans)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Member>> GetPagedAsync(MemberFilter filter, CancellationToken cancellationToken = default)
    {
        var query = context.Members.AsNoTracking();

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

        return new PagedResult<Member>
        {
            Items = items,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Members.AnyAsync(m => m.Email == email, cancellationToken);
    }

    public async Task AddAsync(Member member, CancellationToken cancellationToken = default)
    {
        await context.Members.AddAsync(member, cancellationToken);
    }

    public void Remove(Member member)
    {
        context.Members.Remove(member);
    }
}
