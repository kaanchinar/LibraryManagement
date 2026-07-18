using LibraryManagement.Application.Members.Dtos;
using MediatR;

namespace LibraryManagement.Application.Members.Commands.CreateMember;

public record CreateMemberCommand(CreateMemberDto Dto) : IRequest<MemberDto>;
