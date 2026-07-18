using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Publishers.Commands.CreatePublisher;

public class CreatePublisherCommandHandler : IRequestHandler<CreatePublisherCommand, PublisherDto>
{
    private readonly IAppDbContext _context;

    public CreatePublisherCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PublisherDto> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = request.Dto.ToEntity();
        await _context.Publishers.AddAsync(publisher, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return publisher.ToDto();
    }
}
