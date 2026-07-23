using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Common;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Genre?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Genre>> GetPagedAsync(GenreFilter filter, CancellationToken cancellationToken = default);
    Task AddAsync(Genre genre, CancellationToken cancellationToken = default);
    void Remove(Genre genre);
}
