using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Publishers.Commands.UpdatePublisher;

public class UpdatePublisherCommandHandler : IRequestHandler<UpdatePublisherCommand, PublisherDto>
{
    private readonly IAppDbContext _context;

    public UpdatePublisherCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PublisherDto> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = await _context.Publishers.FindAsync(new object[] { request.Id }, cancellationToken);

        if (publisher is null)
        {
            throw new NotFoundException(nameof(Publisher), request.Id);
        }

        request.Dto.UpdateEntity(publisher);
        _context.Publishers.Update(publisher);
        await _context.SaveChangesAsync(cancellationToken);
        return publisher.ToDto();
    }
}
