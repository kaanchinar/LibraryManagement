using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Authors.Commands.CreateAuthor;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAppDbContext _context;

    public CreateAuthorCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = request.Dto.ToEntity();
        await _context.Authors.AddAsync(author, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return author.ToDto();
    }
}
