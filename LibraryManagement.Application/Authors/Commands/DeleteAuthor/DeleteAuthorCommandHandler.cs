using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Authors.Commands.DeleteAuthor;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Unit>
{
    private readonly IAppDbContext _context;

    public DeleteAuthorCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.Id);
        }

        if (author.Books.Any())
        {
            throw new BusinessRuleException("Cannot delete an author that has books.");
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
