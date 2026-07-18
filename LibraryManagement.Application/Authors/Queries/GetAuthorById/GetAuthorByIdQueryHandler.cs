using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Authors.Queries.GetAuthorById;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    private readonly IAppDbContext _context;

    public GetAuthorByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync(new object[] { request.Id }, cancellationToken);

        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.Id);
        }

        return author.ToDto();
    }
}
