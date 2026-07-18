using MediatR;

namespace LibraryManagement.Application.Genres.Commands.DeleteGenre;

public record DeleteGenreCommand(Guid Id) : IRequest<Unit>;
