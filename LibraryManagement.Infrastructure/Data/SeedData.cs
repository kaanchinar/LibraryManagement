using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Authors.AnyAsync())
        {
            return;
        }

        var author1 = new Author { Id = Guid.NewGuid(), Name = "George Orwell", Bio = "English novelist and essayist." };
        var author2 = new Author { Id = Guid.NewGuid(), Name = "Jane Austen", Bio = "English novelist known for six major novels." };
        var author3 = new Author { Id = Guid.NewGuid(), Name = "Ernest Hemingway", Bio = "American novelist and short-story writer." };

        var genre1 = new Genre { Id = Guid.NewGuid(), Name = "Dystopian" };
        var genre2 = new Genre { Id = Guid.NewGuid(), Name = "Romance" };
        var genre3 = new Genre { Id = Guid.NewGuid(), Name = "Adventure" };

        var publisher1 = new Publisher { Id = Guid.NewGuid(), Name = "Penguin Books" };
        var publisher2 = new Publisher { Id = Guid.NewGuid(), Name = "HarperCollins" };

        var book1 = new Book
        {
            Id = Guid.NewGuid(),
            Title = "1984",
            Isbn = "9780451524935",
            PublicationYear = 1949,
            TotalCopies = 5,
            AvailableCopies = 5,
            AuthorId = author1.Id,
            GenreId = genre1.Id,
            PublisherId = publisher1.Id
        };

        var book2 = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Pride and Prejudice",
            Isbn = "9780141439518",
            PublicationYear = 1813,
            TotalCopies = 3,
            AvailableCopies = 3,
            AuthorId = author2.Id,
            GenreId = genre2.Id,
            PublisherId = publisher2.Id
        };

        var book3 = new Book
        {
            Id = Guid.NewGuid(),
            Title = "The Old Man and the Sea",
            Isbn = "9780684801223",
            PublicationYear = 1952,
            TotalCopies = 4,
            AvailableCopies = 4,
            AuthorId = author3.Id,
            GenreId = genre3.Id,
            PublisherId = publisher1.Id
        };

        var member1 = new Member
        {
            Id = Guid.NewGuid(),
            FullName = "Alice Johnson",
            Email = "alice@example.com",
            PhoneNumber = "+1 555 0101",
            MembershipDate = DateTime.UtcNow.AddMonths(-6),
            IsActive = true
        };

        var member2 = new Member
        {
            Id = Guid.NewGuid(),
            FullName = "Bob Smith",
            Email = "bob@example.com",
            PhoneNumber = "+1 555 0102",
            MembershipDate = DateTime.UtcNow.AddMonths(-3),
            IsActive = true
        };

        await context.Authors.AddRangeAsync(author1, author2, author3);
        await context.Genres.AddRangeAsync(genre1, genre2, genre3);
        await context.Publishers.AddRangeAsync(publisher1, publisher2);
        await context.Books.AddRangeAsync(book1, book2, book3);
        await context.Members.AddRangeAsync(member1, member2);

        await context.SaveChangesAsync();
    }
}
