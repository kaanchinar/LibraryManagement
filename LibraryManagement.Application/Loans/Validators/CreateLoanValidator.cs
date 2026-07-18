using FluentValidation;
using LibraryManagement.Application.Loans.Commands.CreateLoan;

namespace LibraryManagement.Application.Loans.Validators;

public class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.Dto.BookId)
            .NotEmpty().WithMessage("Book is required.");

        RuleFor(x => x.Dto.MemberId)
            .NotEmpty().WithMessage("Member is required.");
    }
}
