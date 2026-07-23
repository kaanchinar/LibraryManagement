using LibraryManagement.Application.Common;
using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Loans.Queries.GetLoanById;

public class GetLoanByIdQueryHandler(ILoanRepository loans)
    : IRequestHandler<GetLoanByIdQuery, LoanDto>
{
    public async Task<LoanDto> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var loan = await loans.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (loan is null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        return loan.ToDto();
    }
}
