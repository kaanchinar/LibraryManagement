using FluentValidation;
using LibraryManagement.Application.Genres.Commands.CreateGenre;

namespace LibraryManagement.Application.Genres.Validators;

public class CreateGenreValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
