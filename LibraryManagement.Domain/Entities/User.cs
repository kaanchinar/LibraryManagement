using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.Member;

    public Guid? MemberId { get; set; }
    public Member? Member { get; set; }
}
