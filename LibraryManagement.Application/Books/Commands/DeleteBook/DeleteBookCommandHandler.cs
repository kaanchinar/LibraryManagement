using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler(IBookRepository books, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBookCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await books.GetByIdWithLoansAsync(request.Id, cancellationToken);

        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.Id);
        }

        if (book.Loans.Any())
        {
            throw new BusinessRuleException("Cannot delete a book that has loans.");
        }

        books.Remove(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
