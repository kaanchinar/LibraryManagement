using LibraryManagement.Domain.Common;

namespace LibraryManagement.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    public Guid AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public Guid? GenreId { get; set; }
    public Genre? Genre { get; set; }

    public Guid? PublisherId { get; set; }
    public Publisher? Publisher { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
