using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandHandler(IGenreRepository genres, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateGenreCommand, GenreDto>
{
    public async Task<GenreDto> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await genres.GetByIdAsync(request.Id, cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(nameof(Genre), request.Id);
        }

        request.Dto.UpdateEntity(genre);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return genre.ToDto();
    }
}
