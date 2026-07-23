using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Authors.Commands.DeleteAuthor;

public class DeleteAuthorCommandHandler(IAuthorRepository authors, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAuthorCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await authors.GetByIdWithBooksAsync(request.Id, cancellationToken);

        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.Id);
        }

        if (author.Books.Any())
        {
            throw new BusinessRuleException("Cannot delete an author that has books.");
        }

        authors.Remove(author);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
