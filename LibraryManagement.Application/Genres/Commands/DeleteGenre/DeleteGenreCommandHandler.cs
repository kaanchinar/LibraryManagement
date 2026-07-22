using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Genres.Commands.DeleteGenre;

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, Unit>
{
    private readonly IAppDbContext _context;

    public DeleteGenreCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _context.Genres
            .Include(g => g.Books)
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(nameof(Genre), request.Id);
        }

        if (genre.Books.Any())
        {
            throw new BusinessRuleException("Cannot delete a genre that has books.");
        }

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
