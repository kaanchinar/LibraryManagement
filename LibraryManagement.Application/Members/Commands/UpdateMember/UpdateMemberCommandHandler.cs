using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Members.Commands.UpdateMember;

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, MemberDto>
{
    private readonly IAppDbContext _context;

    public UpdateMemberCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<MemberDto> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(new object[] { request.Id }, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Member), request.Id);
        }

        request.Dto.UpdateEntity(member);
        _context.Members.Update(member);
        await _context.SaveChangesAsync(cancellationToken);
        return member.ToDto();
    }
}
