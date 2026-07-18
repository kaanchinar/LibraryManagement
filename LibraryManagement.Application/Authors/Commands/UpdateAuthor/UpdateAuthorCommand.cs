using LibraryManagement.Application.Authors.Dtos;
using MediatR;

namespace LibraryManagement.Application.Authors.Commands.UpdateAuthor;

public record UpdateAuthorCommand(Guid Id, UpdateAuthorDto Dto) : IRequest<AuthorDto>;
