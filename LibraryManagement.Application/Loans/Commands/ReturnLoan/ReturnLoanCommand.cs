using MediatR;

namespace LibraryManagement.Application.Loans.Commands.ReturnLoan;

public record ReturnLoanCommand(Guid Id) : IRequest<Unit>;
