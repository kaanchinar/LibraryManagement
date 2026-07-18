using LibraryManagement.Application.Common;

namespace LibraryManagement.Application.Genres.Dtos;

public class GenreFilter : PaginationParams
{
    public string? Name { get; set; }
}
