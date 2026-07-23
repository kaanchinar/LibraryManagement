using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Authors.Commands.CreateAuthor;

public class CreateAuthorCommandHandler(IAuthorRepository authors, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = request.Dto.ToEntity();
        await authors.AddAsync(author, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return author.ToDto();
    }
}
