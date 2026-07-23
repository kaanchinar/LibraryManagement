using LibraryManagement.Application.Common;
using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Publishers.Queries.GetPublisherById;

public class GetPublisherByIdQueryHandler(IPublisherRepository publishers)
    : IRequestHandler<GetPublisherByIdQuery, PublisherDto>
{
    public async Task<PublisherDto> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
    {
        var publisher = await publishers.GetByIdAsync(request.Id, cancellationToken);

        if (publisher is null)
        {
            throw new NotFoundException(nameof(Publisher), request.Id);
        }

        return publisher.ToDto();
    }
}
