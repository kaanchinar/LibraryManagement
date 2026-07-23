using FluentAssertions;
using LibraryManagement.Application.Authors.Commands.DeleteAuthor;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UnitTests.Authors;

public class DeleteAuthorCommandHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Author_Does_Not_Exist()
    {
        await using var context = CreateContext();
        var handler = new DeleteAuthorCommandHandler(new AuthorRepository(context), new UnitOfWork(context));
        var nonExistentId = Guid.NewGuid();

        var act = () => handler.Handle(new DeleteAuthorCommand(nonExistentId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Delete_Author_When_No_Books_Exist()
    {
        await using var context = CreateContext();
        var author = new Author { Name = "Author To Delete" };
        context.Authors.Add(author);
        await context.SaveChangesAsync();

        var handler = new DeleteAuthorCommandHandler(new AuthorRepository(context), new UnitOfWork(context));
        await handler.Handle(new DeleteAuthorCommand(author.Id), CancellationToken.None);

        (await context.Authors.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Throw_BusinessRuleException_When_Author_Has_Books()
    {
        await using var context = CreateContext();
        var author = new Author { Name = "Author With Books" };
        context.Authors.Add(author);
        await context.SaveChangesAsync();

        var book = new Book
        {
            Title = "Some Book",
            Isbn = "1234567890123",
            PublicationYear = 2024,
            TotalCopies = 1,
            AvailableCopies = 1,
            AuthorId = author.Id
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var handler = new DeleteAuthorCommandHandler(new AuthorRepository(context), new UnitOfWork(context));
        var act = () => handler.Handle(new DeleteAuthorCommand(author.Id), CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*books*");
        (await context.Authors.CountAsync()).Should().Be(1);
    }
}
