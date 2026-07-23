using LibraryManagement.Application.Common;
using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Loans.Commands.CreateLoan;

public class CreateLoanCommandHandler(
    IBookRepository books,
    IMemberRepository members,
    ILoanRepository loans,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateLoanCommand, LoanDto>
{
    public async Task<LoanDto> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var book = await books.GetByIdAsync(request.Dto.BookId, cancellationToken);

        if (book is null)
        {
            throw new BusinessRuleException("Book not found.");
        }

        if (book.AvailableCopies <= 0)
        {
            throw new BusinessRuleException("Book is not available for loan.");
        }

        var member = await members.GetByIdAsync(request.Dto.MemberId, cancellationToken);

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

        await loans.AddAsync(loan, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return loan.ToDto();
    }
}
