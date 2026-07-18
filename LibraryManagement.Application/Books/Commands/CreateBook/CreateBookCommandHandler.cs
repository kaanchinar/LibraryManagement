using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Books.Commands.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IAppDbContext _context;

    public CreateBookCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync(new object[] { request.Dto.AuthorId }, cancellationToken);

        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.Dto.AuthorId);
        }

        if (request.Dto.GenreId.HasValue)
        {
            var genre = await _context.Genres.FindAsync(new object[] { request.Dto.GenreId.Value }, cancellationToken);

            if (genre is null)
            {
                throw new NotFoundException(nameof(Genre), request.Dto.GenreId.Value);
            }
        }

        if (request.Dto.PublisherId.HasValue)
        {
            var publisher = await _context.Publishers.FindAsync(new object[] { request.Dto.PublisherId.Value }, cancellationToken);

            if (publisher is null)
            {
                throw new NotFoundException(nameof(Publisher), request.Dto.PublisherId.Value);
            }
        }

        var book = request.Dto.ToEntity();
        await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return book.ToDto();
    }
}
