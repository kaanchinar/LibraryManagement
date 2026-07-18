using MediatR;

namespace LibraryManagement.Application.Publishers.Commands.DeletePublisher;

public record DeletePublisherCommand(Guid Id) : IRequest<Unit>;
