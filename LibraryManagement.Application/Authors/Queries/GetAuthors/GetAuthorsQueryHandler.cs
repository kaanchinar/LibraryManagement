using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Authors.Queries.GetAuthors;

public class GetAuthorsQueryHandler(IAuthorRepository authors)
    : IRequestHandler<GetAuthorsQuery, PagedResult<AuthorDto>>
{
    public async Task<PagedResult<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var result = await authors.GetPagedAsync(request.Filter, cancellationToken);

        return new PagedResult<AuthorDto>
        {
            Items = result.Items.Select(a => a.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
}
