namespace LibraryManagement.Application.Loans.Dtos;

public class LoanDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; }
    public bool IsOverdue { get; set; }
    public DateTime CreatedAt { get; set; }
}
