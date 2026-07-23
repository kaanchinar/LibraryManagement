using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Genres.Queries.GetGenreById;

public class GetGenreByIdQueryHandler(IGenreRepository genres)
    : IRequestHandler<GetGenreByIdQuery, GenreDto>
{
    public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await genres.GetByIdAsync(request.Id, cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(nameof(Genre), request.Id);
        }

        return genre.ToDto();
    }
}
