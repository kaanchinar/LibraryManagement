using LibraryManagement.Application.Authors.Dtos;
using MediatR;

namespace LibraryManagement.Application.Authors.Commands.CreateAuthor;

public record CreateAuthorCommand(CreateAuthorDto Dto) : IRequest<AuthorDto>;
