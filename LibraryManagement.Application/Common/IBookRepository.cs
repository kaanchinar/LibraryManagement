using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Common;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Book?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Book?> GetByIdWithLoansAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Book>> GetPagedAsync(BookFilter filter, CancellationToken cancellationToken = default);
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    void Remove(Book book);
}
