namespace LibraryManagement.Application.Books.Dtos;

public class BookListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int AvailableCopies { get; set; }
    public int TotalCopies { get; set; }
    public string AuthorName { get; set; } = string.Empty;
}
