using LibraryManagement.Application.Members.Dtos;
using MediatR;

namespace LibraryManagement.Application.Members.Commands.UpdateMember;

public record UpdateMemberCommand(Guid Id, UpdateMemberDto Dto) : IRequest<MemberDto>;
