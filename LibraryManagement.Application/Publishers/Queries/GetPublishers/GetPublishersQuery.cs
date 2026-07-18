using LibraryManagement.Application.Common;
using LibraryManagement.Application.Publishers.Dtos;
using MediatR;

namespace LibraryManagement.Application.Publishers.Queries.GetPublishers;

public record GetPublishersQuery(PublisherFilter Filter) : IRequest<PagedResult<PublisherDto>>;
