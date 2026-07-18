using MediatR;

namespace LibraryManagement.Application.Members.Commands.DeleteMember;

public record DeleteMemberCommand(Guid Id) : IRequest<Unit>;
