using LibraryManagement.Domain.Common;

namespace LibraryManagement.Domain.Entities;

public class Member : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
