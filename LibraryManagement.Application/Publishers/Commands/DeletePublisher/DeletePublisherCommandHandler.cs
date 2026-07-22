using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Publishers.Commands.DeletePublisher;

public class DeletePublisherCommandHandler : IRequestHandler<DeletePublisherCommand, Unit>
{
    private readonly IAppDbContext _context;

    public DeletePublisherCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = await _context.Publishers
            .Include(p => p.Books)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (publisher is null)
        {
            throw new NotFoundException(nameof(Publisher), request.Id);
        }

        if (publisher.Books.Any())
        {
            throw new BusinessRuleException("Cannot delete a publisher that has books.");
        }

        _context.Publishers.Remove(publisher);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
