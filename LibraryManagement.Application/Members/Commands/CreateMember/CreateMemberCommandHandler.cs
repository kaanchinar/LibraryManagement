using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Members.Commands.CreateMember;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, MemberDto>
{
    private readonly IAppDbContext _context;

    public CreateMemberCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<MemberDto> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = request.Dto.ToEntity();
        await _context.Members.AddAsync(member, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return member.ToDto();
    }
}
