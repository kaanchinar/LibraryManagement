using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IRefreshTokenService refreshTokenService,
    IRefreshTokenRepository refreshTokens,
    IUnitOfWork unitOfWork,
    AuthTokenIssuer tokenIssuer)
    : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hash = refreshTokenService.HashToken(request.RefreshToken);
        var stored = await refreshTokens.GetByHashWithUserAsync(hash, cancellationToken);

        if (stored is null)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        if (stored.RevokedAt is not null)
        {
            var activeTokens = await refreshTokens.GetActiveByUserIdAsync(stored.UserId, cancellationToken);

            foreach (var t in activeTokens)
            {
                t.RevokedAt = DateTime.UtcNow;
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);

            throw new UnauthorizedException("Refresh token has been revoked.");
        }

        if (stored.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token has expired.");
        }

        stored.RevokedAt = DateTime.UtcNow;
        return await tokenIssuer.IssueAsync(stored.User, cancellationToken);
    }
}
