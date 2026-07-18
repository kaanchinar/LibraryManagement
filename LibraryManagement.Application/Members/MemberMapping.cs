using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Members;

public static class MemberMapping
{
    public static MemberDto ToDto(this Member member)
    {
        return new MemberDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            MembershipDate = member.MembershipDate,
            IsActive = member.IsActive,
            CreatedAt = member.CreatedAt
        };
    }

    public static Member ToEntity(this CreateMemberDto dto)
    {
        return new Member
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            MembershipDate = dto.MembershipDate,
            IsActive = dto.IsActive
        };
    }

    public static void UpdateEntity(this UpdateMemberDto dto, Member member)
    {
        member.FullName = dto.FullName;
        member.Email = dto.Email;
        member.PhoneNumber = dto.PhoneNumber;
        member.MembershipDate = dto.MembershipDate;
        member.IsActive = dto.IsActive;
    }
}
