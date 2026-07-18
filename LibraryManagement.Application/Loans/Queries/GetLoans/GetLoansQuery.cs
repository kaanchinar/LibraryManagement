using LibraryManagement.Application.Common;
using LibraryManagement.Application.Loans.Dtos;
using MediatR;

namespace LibraryManagement.Application.Loans.Queries.GetLoans;

public record GetLoansQuery(LoanFilter Filter) : IRequest<PagedResult<LoanDto>>;
