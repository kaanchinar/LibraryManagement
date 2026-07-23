using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Genres.Commands.DeleteGenre;

public class DeleteGenreCommandHandler(IGenreRepository genres, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteGenreCommand, Unit>
{
    public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await genres.GetByIdWithBooksAsync(request.Id, cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(nameof(Genre), request.Id);
        }

        if (genre.Books.Any())
        {
            throw new BusinessRuleException("Cannot delete a genre that has books.");
        }

        genres.Remove(genre);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
