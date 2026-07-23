using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Loans.Commands.ReturnLoan;

public class ReturnLoanCommandHandler(ILoanRepository loans, IUnitOfWork unitOfWork)
    : IRequestHandler<ReturnLoanCommand, Unit>
{
    public async Task<Unit> Handle(ReturnLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await loans.GetByIdWithBookAsync(request.Id, cancellationToken);

        if (loan is null)
        {
            throw new BusinessRuleException("Loan not found.");
        }

        if (loan.IsReturned)
        {
            throw new BusinessRuleException("Loan has already been returned.");
        }

        loan.ReturnDate = DateTime.UtcNow;
        loan.IsReturned = true;
        loan.Book.AvailableCopies++;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
