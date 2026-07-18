using MediatR;

namespace LibraryManagement.Application.Books.Commands.DeleteBook;

public record DeleteBookCommand(Guid Id) : IRequest<Unit>;
