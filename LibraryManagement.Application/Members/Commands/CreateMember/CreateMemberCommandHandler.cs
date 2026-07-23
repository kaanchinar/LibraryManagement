using LibraryManagement.Application.Common;
using LibraryManagement.Application.Members.Dtos;
using MediatR;

namespace LibraryManagement.Application.Members.Commands.CreateMember;

public class CreateMemberCommandHandler(IMemberRepository members, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMemberCommand, MemberDto>
{
    public async Task<MemberDto> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = request.Dto.ToEntity();
        await members.AddAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return member.ToDto();
    }
}
