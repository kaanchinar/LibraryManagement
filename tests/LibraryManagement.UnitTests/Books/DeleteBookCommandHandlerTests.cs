using FluentAssertions;
using LibraryManagement.Application.Books.Commands.DeleteBook;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UnitTests.Books;

public class DeleteBookCommandHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Book_Does_Not_Exist()
    {
        await using var context = CreateContext();
        var handler = new DeleteBookCommandHandler(context);
        var nonExistentId = Guid.NewGuid();

        var act = () => handler.Handle(new DeleteBookCommand(nonExistentId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Delete_Book_When_No_Loans_Exist()
    {
        await using var context = CreateContext();
        var author = new Author { Name = "Author" };
        context.Authors.Add(author);
        await context.SaveChangesAsync();

        var book = new Book
        {
            Title = "Book To Delete",
            Isbn = "1234567890123",
            PublicationYear = 2024,
            TotalCopies = 1,
            AvailableCopies = 1,
            AuthorId = author.Id
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var handler = new DeleteBookCommandHandler(context);
        await handler.Handle(new DeleteBookCommand(book.Id), CancellationToken.None);

        (await context.Books.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Throw_BusinessRuleException_When_Book_Has_Loans()
    {
        await using var context = CreateContext();

        var author = new Author { Name = "Author" };
        context.Authors.Add(author);

        var member = new Member
        {
            FullName = "Member",
            Email = "member@test.com",
            MembershipDate = DateTime.UtcNow,
            IsActive = true
        };
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var book = new Book
        {
            Title = "Book With Loans",
            Isbn = "1234567890123",
            PublicationYear = 2024,
            TotalCopies = 1,
            AvailableCopies = 0,
            AuthorId = author.Id
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var loan = new Loan
        {
            BookId = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            IsReturned = false
        };
        context.Loans.Add(loan);
        await context.SaveChangesAsync();

        var handler = new DeleteBookCommandHandler(context);
        var act = () => handler.Handle(new DeleteBookCommand(book.Id), CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*loans*");
        (await context.Books.CountAsync()).Should().Be(1);
    }
}
