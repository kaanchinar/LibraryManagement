using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Common;
using MediatR;

namespace LibraryManagement.Application.Authors.Queries.GetAuthors;

public record GetAuthorsQuery(AuthorFilter Filter) : IRequest<PagedResult<AuthorDto>>;
