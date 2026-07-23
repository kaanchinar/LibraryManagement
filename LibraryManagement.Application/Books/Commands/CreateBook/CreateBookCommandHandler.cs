using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Books.Commands.CreateBook;

public class CreateBookCommandHandler(
    IBookRepository books,
    IAuthorRepository authors,
    IGenreRepository genres,
    IPublisherRepository publishers,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateBookCommand, BookDto>
{
    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var author = await authors.GetByIdAsync(request.Dto.AuthorId, cancellationToken);

        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.Dto.AuthorId);
        }

        if (request.Dto.GenreId.HasValue)
        {
            var genre = await genres.GetByIdAsync(request.Dto.GenreId.Value, cancellationToken);

            if (genre is null)
            {
                throw new NotFoundException(nameof(Genre), request.Dto.GenreId.Value);
            }
        }

        if (request.Dto.PublisherId.HasValue)
        {
            var publisher = await publishers.GetByIdAsync(request.Dto.PublisherId.Value, cancellationToken);

            if (publisher is null)
            {
                throw new NotFoundException(nameof(Publisher), request.Dto.PublisherId.Value);
            }
        }

        var book = request.Dto.ToEntity();
        await books.AddAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return book.ToDto();
    }
}
