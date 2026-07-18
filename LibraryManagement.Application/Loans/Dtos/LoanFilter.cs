using LibraryManagement.Application.Common;

namespace LibraryManagement.Application.Loans.Dtos;

public class LoanFilter : PaginationParams
{
    public Guid? MemberId { get; set; }
    public Guid? BookId { get; set; }
    public bool? IsReturned { get; set; }
    public bool? IsOverdue { get; set; }
}
