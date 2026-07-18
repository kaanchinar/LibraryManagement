namespace LibraryManagement.Application.Books.Dtos;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public Guid? GenreId { get; set; }
    public string? GenreName { get; set; }
    public Guid? PublisherId { get; set; }
    public string? PublisherName { get; set; }
    public DateTime CreatedAt { get; set; }
}
