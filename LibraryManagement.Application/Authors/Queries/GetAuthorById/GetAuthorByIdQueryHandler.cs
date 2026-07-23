using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Authors.Queries.GetAuthorById;

public class GetAuthorByIdQueryHandler(IAuthorRepository authors)
    : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await authors.GetByIdAsync(request.Id, cancellationToken);

        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.Id);
        }

        return author.ToDto();
    }
}
