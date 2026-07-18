using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, GenreDto>
{
    private readonly IAppDbContext _context;

    public UpdateGenreCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GenreDto> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _context.Genres.FindAsync(new object[] { request.Id }, cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(nameof(Genre), request.Id);
        }

        request.Dto.UpdateEntity(genre);
        _context.Genres.Update(genre);
        await _context.SaveChangesAsync(cancellationToken);
        return genre.ToDto();
    }
}
