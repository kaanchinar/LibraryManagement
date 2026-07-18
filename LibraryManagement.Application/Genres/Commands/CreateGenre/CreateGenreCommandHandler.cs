using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreDto>
{
    private readonly IAppDbContext _context;

    public CreateGenreCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = request.Dto.ToEntity();
        await _context.Genres.AddAsync(genre, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return genre.ToDto();
    }
}
