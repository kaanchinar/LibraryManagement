using LibraryManagement.Application.Books.Dtos;
using MediatR;

namespace LibraryManagement.Application.Books.Commands.UpdateBook;

public record UpdateBookCommand(Guid Id, UpdateBookDto Dto) : IRequest<BookDto>;
