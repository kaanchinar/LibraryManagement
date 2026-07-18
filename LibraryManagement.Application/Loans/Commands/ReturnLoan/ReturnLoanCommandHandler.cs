using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Loans.Commands.ReturnLoan;

public class ReturnLoanCommandHandler : IRequestHandler<ReturnLoanCommand, Unit>
{
    private readonly IAppDbContext _context;

    public ReturnLoanCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ReturnLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _context.Loans.AsNoTracking()
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

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

        _context.Loans.Update(loan);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
