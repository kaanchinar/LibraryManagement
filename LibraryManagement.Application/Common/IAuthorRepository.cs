using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Common;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Author?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Author>> GetPagedAsync(AuthorFilter filter, CancellationToken cancellationToken = default);
    Task AddAsync(Author author, CancellationToken cancellationToken = default);
    void Remove(Author author);
}
