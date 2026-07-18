using LibraryManagement.Application.Loans.Dtos;
using MediatR;

namespace LibraryManagement.Application.Loans.Commands.CreateLoan;

public record CreateLoanCommand(CreateLoanDto Dto) : IRequest<LoanDto>;
