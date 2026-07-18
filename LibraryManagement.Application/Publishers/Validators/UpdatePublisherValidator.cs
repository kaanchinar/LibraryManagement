using FluentValidation;
using LibraryManagement.Application.Publishers.Commands.UpdatePublisher;

namespace LibraryManagement.Application.Publishers.Validators;

public class UpdatePublisherValidator : AbstractValidator<UpdatePublisherCommand>
{
    public UpdatePublisherValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}
