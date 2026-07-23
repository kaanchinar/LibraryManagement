using FluentAssertions;
using LibraryManagement.Application.Genres.Commands.DeleteGenre;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UnitTests.Genres;

public class DeleteGenreCommandHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Genre_Does_Not_Exist()
    {
        await using var context = CreateContext();
        var handler = new DeleteGenreCommandHandler(new GenreRepository(context), new UnitOfWork(context));
        var nonExistentId = Guid.NewGuid();

        var act = () => handler.Handle(new DeleteGenreCommand(nonExistentId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Delete_Genre_When_No_Books_Exist()
    {
        await using var context = CreateContext();
        var genre = new Genre { Name = "Genre To Delete" };
        context.Genres.Add(genre);
        await context.SaveChangesAsync();

        var handler = new DeleteGenreCommandHandler(new GenreRepository(context), new UnitOfWork(context));
        await handler.Handle(new DeleteGenreCommand(genre.Id), CancellationToken.None);

        (await context.Genres.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Throw_BusinessRuleException_When_Genre_Has_Books()
    {
        await using var context = CreateContext();

        var author = new Author { Name = "Author" };
        context.Authors.Add(author);

        var genre = new Genre { Name = "Genre With Books" };
        context.Genres.Add(genre);
        await context.SaveChangesAsync();

        var book = new Book
        {
            Title = "Some Book",
            Isbn = "1234567890123",
            PublicationYear = 2024,
            TotalCopies = 1,
            AvailableCopies = 1,
            AuthorId = author.Id,
            GenreId = genre.Id
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var handler = new DeleteGenreCommandHandler(new GenreRepository(context), new UnitOfWork(context));
        var act = () => handler.Handle(new DeleteGenreCommand(genre.Id), CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*books*");
        (await context.Genres.CountAsync()).Should().Be(1);
    }
}
