using LibraryManagement.Application.Common;
using LibraryManagement.Application.Publishers.Dtos;
using MediatR;

namespace LibraryManagement.Application.Publishers.Commands.CreatePublisher;

public class CreatePublisherCommandHandler(IPublisherRepository publishers, IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePublisherCommand, PublisherDto>
{
    public async Task<PublisherDto> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = request.Dto.ToEntity();
        await publishers.AddAsync(publisher, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return publisher.ToDto();
    }
}
