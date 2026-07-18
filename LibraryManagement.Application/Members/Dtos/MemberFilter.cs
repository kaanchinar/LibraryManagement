using LibraryManagement.Application.Common;

namespace LibraryManagement.Application.Members.Dtos;

public class MemberFilter : PaginationParams
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}
