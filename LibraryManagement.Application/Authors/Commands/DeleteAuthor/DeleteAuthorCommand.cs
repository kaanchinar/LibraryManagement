using MediatR;

namespace LibraryManagement.Application.Authors.Commands.DeleteAuthor;

public record DeleteAuthorCommand(Guid Id) : IRequest<Unit>;
