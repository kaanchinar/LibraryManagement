using FluentValidation;
using LibraryManagement.Application.Books.Commands.CreateBook;

namespace LibraryManagement.Application.Books.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(300).WithMessage("Title must not exceed 300 characters.");

        RuleFor(x => x.Dto.Isbn)
            .NotEmpty().WithMessage("ISBN is required.")
            .MaximumLength(13).WithMessage("ISBN must not exceed 13 characters.")
            .Must(Isbn => Isbn.Length == 10 || Isbn.Length == 13)
            .WithMessage("ISBN must be either 10 or 13 characters long.");

        RuleFor(x => x.Dto.PublicationYear)
            .InclusiveBetween(1000, DateTime.UtcNow.Year + 1)
            .WithMessage($"Publication year must be between 1000 and {DateTime.UtcNow.Year + 1}.");

        RuleFor(x => x.Dto.TotalCopies)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Total copies must be at least 1.");

        RuleFor(x => x.Dto.AuthorId)
            .NotEmpty().WithMessage("Author is required.");
    }
}
