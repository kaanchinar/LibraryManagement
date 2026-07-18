using LibraryManagement.Application.Common;

namespace LibraryManagement.Application.Publishers.Dtos;

public class PublisherFilter : PaginationParams
{
    public string? Name { get; set; }
}
