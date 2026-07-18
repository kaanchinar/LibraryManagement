using LibraryManagement.Application.Genres.Dtos;
using MediatR;

namespace LibraryManagement.Application.Genres.Commands.CreateGenre;

public record CreateGenreCommand(CreateGenreDto Dto) : IRequest<GenreDto>;
