using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Genres.Queries.GetGenreById;

public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreDto>
{
    private readonly IAppDbContext _context;

    public GetGenreByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await _context.Genres.FindAsync(new object[] { request.Id }, cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(nameof(Genre), request.Id);
        }

        return genre.ToDto();
    }
}
