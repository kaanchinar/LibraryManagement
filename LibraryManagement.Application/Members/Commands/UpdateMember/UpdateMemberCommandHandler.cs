using LibraryManagement.Application.Common;
using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Members.Commands.UpdateMember;

public class UpdateMemberCommandHandler(IMemberRepository members, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMemberCommand, MemberDto>
{
    public async Task<MemberDto> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await members.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Member), request.Id);
        }

        request.Dto.UpdateEntity(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return member.ToDto();
    }
}
