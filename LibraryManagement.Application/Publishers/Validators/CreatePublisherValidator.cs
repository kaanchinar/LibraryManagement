using FluentValidation;
using LibraryManagement.Application.Publishers.Commands.CreatePublisher;

namespace LibraryManagement.Application.Publishers.Validators;

public class CreatePublisherValidator : AbstractValidator<CreatePublisherCommand>
{
    public CreatePublisherValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}
