using FluentAssertions;
using LibraryManagement.Application.Books.Commands.CreateBook;
using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Books.Validators;

namespace LibraryManagement.UnitTests.Books;

public class CreateBookValidatorTests
{
    private readonly CreateBookValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var dto = new CreateBookDto { Title = "", Isbn = "1234567890", PublicationYear = 2020, TotalCopies = 1, AuthorId = Guid.NewGuid() };
        var command = new CreateBookCommand(dto);
        var result = _validator.Validate(command);
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Title");
    }

    [Fact]
    public void Should_Have_Error_When_Isbn_Is_Invalid_Length()
    {
        var dto = new CreateBookDto { Title = "Book", Isbn = "123", PublicationYear = 2020, TotalCopies = 1, AuthorId = Guid.NewGuid() };
        var command = new CreateBookCommand(dto);
        var result = _validator.Validate(command);
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Isbn");
    }

    [Fact]
    public void Should_Have_Error_When_PublicationYear_Is_In_Future()
    {
        var dto = new CreateBookDto { Title = "Book", Isbn = "1234567890", PublicationYear = DateTime.UtcNow.Year + 2, TotalCopies = 1, AuthorId = Guid.NewGuid() };
        var command = new CreateBookCommand(dto);
        var result = _validator.Validate(command);
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.PublicationYear");
    }

    [Fact]
    public void Should_Not_Have_Errors_For_Valid_Dto()
    {
        var dto = new CreateBookDto { Title = "Book", Isbn = "1234567890", PublicationYear = 2020, TotalCopies = 1, AuthorId = Guid.NewGuid() };
        var command = new CreateBookCommand(dto);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
