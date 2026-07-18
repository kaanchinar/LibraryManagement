using FluentValidation;
using LibraryManagement.Application.Genres.Commands.UpdateGenre;

namespace LibraryManagement.Application.Genres.Validators;

public class UpdateGenreValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
