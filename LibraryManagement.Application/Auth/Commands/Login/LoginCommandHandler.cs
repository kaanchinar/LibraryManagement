using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Auth.Commands.Login;

public class LoginCommandHandler(IAppDbContext context, IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var token = jwtTokenService.GenerateToken(user);
        return new AuthResponse(token.Token, token.ExpiresAt);
    }
}