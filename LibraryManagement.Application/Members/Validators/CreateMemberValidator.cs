using FluentValidation;
using LibraryManagement.Application.Members.Commands.CreateMember;

namespace LibraryManagement.Application.Members.Validators;

public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.Dto.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200).WithMessage("Full name must not exceed 200 characters.");

        RuleFor(x => x.Dto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        RuleFor(x => x.Dto.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

        RuleFor(x => x.Dto.MembershipDate)
            .NotEmpty().WithMessage("Membership date is required.");
    }
}
