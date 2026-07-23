using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Members.Commands.DeleteMember;

public class DeleteMemberCommandHandler(IMemberRepository members, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteMemberCommand, Unit>
{
    public async Task<Unit> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await members.GetByIdWithLoansAsync(request.Id, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Member), request.Id);
        }

        if (member.Loans.Any())
        {
            throw new BusinessRuleException("Cannot delete a member that has loans.");
        }

        members.Remove(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
