using LibraryManagement.Application.Genres.Dtos;
using MediatR;

namespace LibraryManagement.Application.Genres.Commands.UpdateGenre;

public record UpdateGenreCommand(Guid Id, UpdateGenreDto Dto) : IRequest<GenreDto>;
