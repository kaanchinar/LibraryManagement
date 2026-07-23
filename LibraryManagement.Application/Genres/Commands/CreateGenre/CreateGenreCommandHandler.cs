using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres.Dtos;
using MediatR;

namespace LibraryManagement.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler(IGenreRepository genres, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGenreCommand, GenreDto>
{
    public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = request.Dto.ToEntity();
        await genres.AddAsync(genre, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return genre.ToDto();
    }
}
