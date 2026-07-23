using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Auth;

public class AuthTokenIssuer(
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService,
    IRefreshTokenRepository refreshTokens,
    IUnitOfWork unitOfWork)
{
    public async Task<AuthResponse> IssueAsync(User user, CancellationToken cancellationToken = default)
    {
        var token = jwtTokenService.GenerateToken(user);
        var refreshToken = refreshTokenService.GenerateToken();

        await refreshTokens.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = refreshTokenService.HashToken(refreshToken.Token),
            ExpiresAt = refreshToken.ExpiresAt,
            UserId = user.Id
        }, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse(token.Token, token.ExpiresAt, refreshToken.Token);
    }
}
