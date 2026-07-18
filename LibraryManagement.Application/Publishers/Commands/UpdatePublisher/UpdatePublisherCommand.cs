using LibraryManagement.Application.Publishers.Dtos;
using MediatR;

namespace LibraryManagement.Application.Publishers.Commands.UpdatePublisher;

public record UpdatePublisherCommand(Guid Id, UpdatePublisherDto Dto) : IRequest<PublisherDto>;
