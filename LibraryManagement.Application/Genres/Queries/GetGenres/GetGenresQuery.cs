using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres.Dtos;
using MediatR;

namespace LibraryManagement.Application.Genres.Queries.GetGenres;

public record GetGenresQuery(GenreFilter Filter) : IRequest<PagedResult<GenreDto>>;
