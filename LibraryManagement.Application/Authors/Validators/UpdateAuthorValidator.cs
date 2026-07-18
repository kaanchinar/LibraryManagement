using FluentValidation;
using LibraryManagement.Application.Authors.Commands.UpdateAuthor;

namespace LibraryManagement.Application.Authors.Validators;

public class UpdateAuthorValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Dto.Bio)
            .MaximumLength(2000).WithMessage("Bio must not exceed 2000 characters.");
    }
}
