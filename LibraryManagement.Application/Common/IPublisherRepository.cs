using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Common;

public interface IPublisherRepository
{
    Task<Publisher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Publisher?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Publisher>> GetPagedAsync(PublisherFilter filter, CancellationToken cancellationToken = default);
    Task AddAsync(Publisher publisher, CancellationToken cancellationToken = default);
    void Remove(Publisher publisher);
}
