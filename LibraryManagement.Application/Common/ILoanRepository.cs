using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Common;

public interface ILoanRepository
{
    Task<Loan?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Loan?> GetByIdWithBookAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Loan>> GetPagedAsync(LoanFilter filter, CancellationToken cancellationToken = default);
    Task AddAsync(Loan loan, CancellationToken cancellationToken = default);
}
