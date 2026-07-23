using LibraryManagement.Application.Common;
using LibraryManagement.Application.Members.Dtos;
using MediatR;

namespace LibraryManagement.Application.Members.Queries.GetMembers;

public class GetMembersQueryHandler(IMemberRepository members)
    : IRequestHandler<GetMembersQuery, PagedResult<MemberDto>>
{
    public async Task<PagedResult<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        var result = await members.GetPagedAsync(request.Filter, cancellationToken);

        return new PagedResult<MemberDto>
        {
            Items = result.Items.Select(m => m.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
}
