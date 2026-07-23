using LibraryManagement.Application.Common;
using LibraryManagement.Application.Publishers.Dtos;
using MediatR;

namespace LibraryManagement.Application.Publishers.Queries.GetPublishers;

public class GetPublishersQueryHandler(IPublisherRepository publishers)
    : IRequestHandler<GetPublishersQuery, PagedResult<PublisherDto>>
{
    public async Task<PagedResult<PublisherDto>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
    {
        var result = await publishers.GetPagedAsync(request.Filter, cancellationToken);

        return new PagedResult<PublisherDto>
        {
            Items = result.Items.Select(p => p.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
}
