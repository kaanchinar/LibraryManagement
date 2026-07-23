using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler(
    IBookRepository books,
    IAuthorRepository authors,
    IGenreRepository genres,
    IPublisherRepository publishers,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBookCommand, BookDto>
{
    public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await books.GetByIdAsync(request.Id, cancellationToken);

        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.Id);
        }

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

        request.Dto.UpdateEntity(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return book.ToDto();
    }
}
