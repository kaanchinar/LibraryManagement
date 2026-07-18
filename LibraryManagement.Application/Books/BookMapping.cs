using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Books;

public static class BookMapping
{
    public static BookDto ToDto(this Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            PublicationYear = book.PublicationYear,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            AuthorId = book.AuthorId,
            AuthorName = book.Author.Name,
            GenreId = book.GenreId,
            GenreName = book.Genre?.Name,
            PublisherId = book.PublisherId,
            PublisherName = book.Publisher?.Name,
            CreatedAt = book.CreatedAt
        };
    }

    public static BookListDto ToListDto(this Book book)
    {
        return new BookListDto
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            PublicationYear = book.PublicationYear,
            AvailableCopies = book.AvailableCopies,
            TotalCopies = book.TotalCopies,
            AuthorName = book.Author.Name
        };
    }

    public static Book ToEntity(this CreateBookDto dto)
    {
        return new Book
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Isbn = dto.Isbn,
            PublicationYear = dto.PublicationYear,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.TotalCopies,
            AuthorId = dto.AuthorId,
            GenreId = dto.GenreId,
            PublisherId = dto.PublisherId
        };
    }

    public static void UpdateEntity(this UpdateBookDto dto, Book book)
    {
        book.Title = dto.Title;
        book.Isbn = dto.Isbn;
        book.PublicationYear = dto.PublicationYear;
        book.TotalCopies = dto.TotalCopies;
        book.AuthorId = dto.AuthorId;
        book.GenreId = dto.GenreId;
        book.PublisherId = dto.PublisherId;
    }
}
