using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
{
    private readonly IAppDbContext _context;

    public DeleteBookCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);

        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.Id);
        }

        if (book.Loans.Any())
        {
            throw new BusinessRuleException("Cannot delete a book that has loans.");
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
