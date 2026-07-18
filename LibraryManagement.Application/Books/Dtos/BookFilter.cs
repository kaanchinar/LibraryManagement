using LibraryManagement.Application.Common;

namespace LibraryManagement.Application.Books.Dtos;

public class BookFilter : PaginationParams
{
    public string? Title { get; set; }
    public string? Isbn { get; set; }
    public Guid? AuthorId { get; set; }
    public Guid? GenreId { get; set; }
    public Guid? PublisherId { get; set; }
    public int? PublicationYear { get; set; }
    public bool? IsAvailable { get; set; }
}
