using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Loans.Commands.CreateLoan;

public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanDto>
{
    private readonly IAppDbContext _context;

    public CreateLoanCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<LoanDto> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { request.Dto.BookId }, cancellationToken);

        if (book is null)
        {
            throw new BusinessRuleException("Book not found.");
        }

        if (book.AvailableCopies <= 0)
        {
            throw new BusinessRuleException("Book is not available for loan.");
        }

        var member = await _context.Members.FindAsync(new object[] { request.Dto.MemberId }, cancellationToken);

        if (member is null)
        {
            throw new BusinessRuleException("Member not found.");
        }

        if (!member.IsActive)
        {
            throw new BusinessRuleException("Member is not active.");
        }

        var loan = request.Dto.ToEntity(book, member);
        book.AvailableCopies--;

        await _context.Loans.AddAsync(loan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return loan.ToDto();
    }
}
