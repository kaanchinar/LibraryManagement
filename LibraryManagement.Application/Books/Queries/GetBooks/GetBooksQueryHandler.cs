using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Books.Queries.GetBooks;

public class GetBooksQueryHandler(IBookRepository books)
    : IRequestHandler<GetBooksQuery, PagedResult<BookListDto>>
{
    public async Task<PagedResult<BookListDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var result = await books.GetPagedAsync(request.Filter, cancellationToken);

        return new PagedResult<BookListDto>
        {
            Items = result.Items.Select(b => b.ToListDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
}
