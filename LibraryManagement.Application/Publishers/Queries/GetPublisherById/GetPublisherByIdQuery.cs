using LibraryManagement.Application.Publishers.Dtos;
using MediatR;

namespace LibraryManagement.Application.Publishers.Queries.GetPublisherById;

public record GetPublisherByIdQuery(Guid Id) : IRequest<PublisherDto>;
