namespace LibraryManagement.Application.Loans.Dtos;

public class CreateLoanDto
{
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
}
