namespace LibraryManagement.Application.Books.Dtos;

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int TotalCopies { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? GenreId { get; set; }
    public Guid? PublisherId { get; set; }
}
