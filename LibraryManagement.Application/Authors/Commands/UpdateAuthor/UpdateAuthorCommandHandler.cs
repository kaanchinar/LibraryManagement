using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
{
    private readonly IAppDbContext _context;

    public UpdateAuthorCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AuthorDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync(new object[] { request.Id }, cancellationToken);

        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.Id);
        }

        request.Dto.UpdateEntity(author);
        _context.Authors.Update(author);
        await _context.SaveChangesAsync(cancellationToken);
        return author.ToDto();
    }
}
