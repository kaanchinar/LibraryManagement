using LibraryManagement.Application.Publishers.Dtos;
using MediatR;

namespace LibraryManagement.Application.Publishers.Commands.CreatePublisher;

public record CreatePublisherCommand(CreatePublisherDto Dto) : IRequest<PublisherDto>;
