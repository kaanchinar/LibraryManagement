namespace LibraryManagement.Application.Members.Dtos;

public class CreateMemberDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; } = true;
}
