using LibraryManagement.Application.Books.Dtos;
using MediatR;

namespace LibraryManagement.Application.Books.Commands.CreateBook;

public record CreateBookCommand(CreateBookDto Dto) : IRequest<BookDto>;
