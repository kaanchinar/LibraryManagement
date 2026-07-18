using LibraryManagement.Domain.Common;

namespace LibraryManagement.Domain.Entities;

public class Loan : BaseEntity
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; }
}
