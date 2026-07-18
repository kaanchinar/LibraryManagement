using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Books.Queries.GetBooks;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PagedResult<BookListDto>>
{
    private readonly IAppDbContext _context;

    public GetBooksQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<BookListDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        IQueryable<Book> query = _context.Books.AsNoTracking()
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

        return new PagedResult<BookListDto>
        {
            Items = items.Select(b => b.ToListDto()).ToList(),
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }
}
