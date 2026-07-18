using LibraryManagement.Application.Books.Dtos;
using MediatR;

namespace LibraryManagement.Application.Books.Queries.GetBookById;

public record GetBookByIdQuery(Guid Id) : IRequest<BookDto>;
