using LibraryManagement.Application.Common;

namespace LibraryManagement.Application.Authors.Dtos;

public class AuthorFilter : PaginationParams
{
    public string? Name { get; set; }
}
