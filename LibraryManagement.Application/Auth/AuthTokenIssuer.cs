using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Auth;

public class AuthTokenIssuer(
    IAppDbContext context,
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService)
{
    public async Task<AuthResponse> IssueAsync(User user, CancellationToken cancellationToken = default)
    {
        var token = jwtTokenService.GenerateToken(user);
        var refreshToken = refreshTokenService.GenerateToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = refreshTokenService.HashToken(refreshToken.Token),
            ExpiresAt = refreshToken.ExpiresAt,
            UserId = user.Id
        });
        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponse(token.Token, token.ExpiresAt, refreshToken.Token);
    }
}
