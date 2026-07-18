using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Books.Queries.GetBooks;

public record GetBooksQuery(BookFilter Filter) : IRequest<PagedResult<BookListDto>>;
