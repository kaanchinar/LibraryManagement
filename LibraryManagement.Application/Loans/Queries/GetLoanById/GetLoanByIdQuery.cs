using LibraryManagement.Application.Loans.Dtos;
using MediatR;

namespace LibraryManagement.Application.Loans.Queries.GetLoanById;

public record GetLoanByIdQuery(Guid Id) : IRequest<LoanDto>;
