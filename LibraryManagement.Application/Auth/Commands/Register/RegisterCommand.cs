using LibraryManagement.Application.Auth.Dtos;
using MediatR;

namespace LibraryManagement.Application.Auth.Commands.Register;

public record RegisterCommand(string FullName, string Email, string Password) : IRequest<AuthResponse>;
