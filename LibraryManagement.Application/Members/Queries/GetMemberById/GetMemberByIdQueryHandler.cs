using LibraryManagement.Application.Common;
using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Members.Queries.GetMemberById;

public class GetMemberByIdQueryHandler(IMemberRepository members)
    : IRequestHandler<GetMemberByIdQuery, MemberDto>
{
    public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await members.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Member), request.Id);
        }

        return member.ToDto();
    }
}
