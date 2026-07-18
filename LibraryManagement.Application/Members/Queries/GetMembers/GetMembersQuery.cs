using LibraryManagement.Application.Common;
using LibraryManagement.Application.Members.Dtos;
using MediatR;

namespace LibraryManagement.Application.Members.Queries.GetMembers;

public record GetMembersQuery(MemberFilter Filter) : IRequest<PagedResult<MemberDto>>;
