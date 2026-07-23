using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres.Dtos;
using MediatR;

namespace LibraryManagement.Application.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(IGenreRepository genres)
    : IRequestHandler<GetGenresQuery, PagedResult<GenreDto>>
{
    public async Task<PagedResult<GenreDto>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        var result = await genres.GetPagedAsync(request.Filter, cancellationToken);

        return new PagedResult<GenreDto>
        {
            Items = result.Items.Select(g => g.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
}
