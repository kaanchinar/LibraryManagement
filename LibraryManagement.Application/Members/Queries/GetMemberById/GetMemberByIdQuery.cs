using LibraryManagement.Application.Members.Dtos;
using MediatR;

namespace LibraryManagement.Application.Members.Queries.GetMemberById;

public record GetMemberByIdQuery(Guid Id) : IRequest<MemberDto>;
