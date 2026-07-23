using LibraryManagement.Application.Common;
using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Publishers.Commands.UpdatePublisher;

public class UpdatePublisherCommandHandler(IPublisherRepository publishers, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePublisherCommand, PublisherDto>
{
    public async Task<PublisherDto> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = await publishers.GetByIdAsync(request.Id, cancellationToken);

        if (publisher is null)
        {
            throw new NotFoundException(nameof(Publisher), request.Id);
        }

        request.Dto.UpdateEntity(publisher);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return publisher.ToDto();
    }
}
