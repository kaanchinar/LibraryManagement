using LibraryManagement.Application.Auth.Dtos;

using MediatR;

namespace LibraryManagement.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;