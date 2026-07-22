using FluentAssertions;
using LibraryManagement.Application.Publishers.Commands.DeletePublisher;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UnitTests.Publishers;

public class DeletePublisherCommandHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Publisher_Does_Not_Exist()
    {
        await using var context = CreateContext();
        var handler = new DeletePublisherCommandHandler(context);
        var nonExistentId = Guid.NewGuid();

        var act = () => handler.Handle(new DeletePublisherCommand(nonExistentId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Delete_Publisher_When_No_Books_Exist()
    {
        await using var context = CreateContext();
        var publisher = new Publisher { Name = "Publisher To Delete" };
        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();

        var handler = new DeletePublisherCommandHandler(context);
        await handler.Handle(new DeletePublisherCommand(publisher.Id), CancellationToken.None);

        (await context.Publishers.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Throw_BusinessRuleException_When_Publisher_Has_Books()
    {
        await using var context = CreateContext();

        var author = new Author { Name = "Author" };
        context.Authors.Add(author);

        var publisher = new Publisher { Name = "Publisher With Books" };
        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();

        var book = new Book
        {
            Title = "Some Book",
            Isbn = "1234567890123",
            PublicationYear = 2024,
            TotalCopies = 1,
            AvailableCopies = 1,
            AuthorId = author.Id,
            PublisherId = publisher.Id
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var handler = new DeletePublisherCommandHandler(context);
        var act = () => handler.Handle(new DeletePublisherCommand(publisher.Id), CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*books*");
        (await context.Publishers.CountAsync()).Should().Be(1);
    }
}
