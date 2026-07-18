using LibraryManagement.Application.Authors.Dtos;
using MediatR;

namespace LibraryManagement.Application.Authors.Queries.GetAuthorById;

public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDto>;
