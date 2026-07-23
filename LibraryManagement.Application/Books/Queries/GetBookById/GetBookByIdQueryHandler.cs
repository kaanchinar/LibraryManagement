using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler(IBookRepository books)
    : IRequestHandler<GetBookByIdQuery, BookDto>
{
    public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await books.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.Id);
        }

        return book.ToDto();
    }
}
