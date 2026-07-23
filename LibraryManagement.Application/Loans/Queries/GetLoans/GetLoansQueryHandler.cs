using LibraryManagement.Application.Common;
using LibraryManagement.Application.Loans.Dtos;
using MediatR;

namespace LibraryManagement.Application.Loans.Queries.GetLoans;

public class GetLoansQueryHandler(ILoanRepository loans)
    : IRequestHandler<GetLoansQuery, PagedResult<LoanDto>>
{
    public async Task<PagedResult<LoanDto>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
    {
        var result = await loans.GetPagedAsync(request.Filter, cancellationToken);

        return new PagedResult<LoanDto>
        {
            Items = result.Items.Select(l => l.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
}
