using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Members.Queries.GetMemberById;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto>
{
    private readonly IAppDbContext _context;

    public GetMemberByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(new object[] { request.Id }, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Member), request.Id);
        }

        return member.ToDto();
    }
}
