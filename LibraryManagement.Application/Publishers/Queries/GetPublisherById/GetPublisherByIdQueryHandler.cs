using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Publishers.Queries.GetPublisherById;

public class GetPublisherByIdQueryHandler : IRequestHandler<GetPublisherByIdQuery, PublisherDto>
{
    private readonly IAppDbContext _context;

    public GetPublisherByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PublisherDto> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
    {
        var publisher = await _context.Publishers.FindAsync(new object[] { request.Id }, cancellationToken);

        if (publisher is null)
        {
            throw new NotFoundException(nameof(Publisher), request.Id);
        }

        return publisher.ToDto();
    }
}
