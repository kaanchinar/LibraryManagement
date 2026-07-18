using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Loans.Queries.GetLoanById;

public class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, LoanDto>
{
    private readonly IAppDbContext _context;

    public GetLoanByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<LoanDto> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var loan = await _context.Loans.AsNoTracking()
            .Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (loan is null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        return loan.ToDto();
    }
}
