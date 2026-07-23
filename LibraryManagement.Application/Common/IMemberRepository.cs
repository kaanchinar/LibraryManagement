using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Common;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Member?> GetByIdWithLoansAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Member>> GetPagedAsync(MemberFilter filter, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Member member, CancellationToken cancellationToken = default);
    void Remove(Member member);
}
