using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommandHandler(IAuthorRepository authors, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateAuthorCommand, AuthorDto>
{
    public async Task<AuthorDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await authors.GetByIdAsync(request.Id, cancellationToken);

        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.Id);
        }

        request.Dto.UpdateEntity(author);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return author.ToDto();
    }
}
