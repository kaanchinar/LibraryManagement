using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class BookRepository(AppDbContext context) : IBookRepository
{
    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Books.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Book?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Books.AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<Book?> GetByIdWithLoansAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Books
            .Include(b => b.Loans)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Book>> GetPagedAsync(BookFilter filter, CancellationToken cancellationToken = default)
    {
        IQueryable<Book> query = context.Books.AsNoTracking()
            .Include(b => b.Author);

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(b => b.Title.Contains(filter.Title));
        }

        if (!string.IsNullOrWhiteSpace(filter.Isbn))
        {
            query = query.Where(b => b.Isbn.Contains(filter.Isbn));
        }

        if (filter.AuthorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == filter.AuthorId.Value);
        }

        if (filter.GenreId.HasValue)
        {
            query = query.Where(b => b.GenreId == filter.GenreId.Value);
        }

        if (filter.PublisherId.HasValue)
        {
            query = query.Where(b => b.PublisherId == filter.PublisherId.Value);
        }

        if (filter.PublicationYear.HasValue)
        {
            query = query.Where(b => b.PublicationYear == filter.PublicationYear.Value);
        }

        if (filter.IsAvailable.HasValue)
        {
            query = filter.IsAvailable.Value
                ? query.Where(b => b.AvailableCopies > 0)
                : query.Where(b => b.AvailableCopies == 0);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "title" => filter.SortDescending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
            "publicationyear" => filter.SortDescending ? query.OrderByDescending(b => b.PublicationYear) : query.OrderBy(b => b.PublicationYear),
            _ => filter.SortDescending ? query.OrderByDescending(b => b.CreatedAt) : query.OrderBy(b => b.CreatedAt)
        };

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Book>
        {
            Items = items,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await context.Books.AddAsync(book, cancellationToken);
    }

    public void Remove(Book book)
    {
        context.Books.Remove(book);
    }
}
