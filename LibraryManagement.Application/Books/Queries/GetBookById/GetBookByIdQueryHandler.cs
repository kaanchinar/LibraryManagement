using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
{
    private readonly IAppDbContext _context;

    public GetBookByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.Id);
        }

        return book.ToDto();
    }
}
