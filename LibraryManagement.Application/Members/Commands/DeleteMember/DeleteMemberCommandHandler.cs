using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Members.Commands.DeleteMember;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Unit>
{
    private readonly IAppDbContext _context;

    public DeleteMemberCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(new object[] { request.Id }, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Member), request.Id);
        }

        if (member.Loans.Any())
        {
            throw new BusinessRuleException("Cannot delete a member that has loans.");
        }

        _context.Members.Remove(member);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
