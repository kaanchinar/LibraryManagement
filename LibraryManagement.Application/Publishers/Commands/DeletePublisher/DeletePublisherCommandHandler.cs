using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Publishers.Commands.DeletePublisher;

public class DeletePublisherCommandHandler(IPublisherRepository publishers, IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePublisherCommand, Unit>
{
    public async Task<Unit> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = await publishers.GetByIdWithBooksAsync(request.Id, cancellationToken);

        if (publisher is null)
        {
            throw new NotFoundException(nameof(Publisher), request.Id);
        }

        if (publisher.Books.Any())
        {
            throw new BusinessRuleException("Cannot delete a publisher that has books.");
        }

        publishers.Remove(publisher);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
