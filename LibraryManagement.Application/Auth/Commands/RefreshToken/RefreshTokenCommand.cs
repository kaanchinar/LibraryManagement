using LibraryManagement.Application.Auth.Dtos;
using MediatR;

namespace LibraryManagement.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;
