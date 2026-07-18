using LibraryManagement.Application.Genres.Dtos;
using MediatR;

namespace LibraryManagement.Application.Genres.Queries.GetGenreById;

public record GetGenreByIdQuery(Guid Id) : IRequest<GenreDto>;
