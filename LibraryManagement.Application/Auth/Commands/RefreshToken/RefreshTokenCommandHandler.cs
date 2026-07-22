using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IAppDbContext context,
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hash = refreshTokenService.HashToken(request.RefreshToken);

        var stored = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.TokenHash == hash, cancellationToken);

        if (stored is null)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        if (stored.RevokedAt is not null)
        {
            var activeTokens = await context.RefreshTokens
                .Where(r => r.UserId == stored.UserId && r.RevokedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var t in activeTokens)
            {
                t.RevokedAt = DateTime.UtcNow;
            }
            await context.SaveChangesAsync(cancellationToken);

            throw new UnauthorizedException("Refresh token has been revoked.");
        }

        if (stored.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token has expired.");
        }

        stored.RevokedAt = DateTime.UtcNow;

        var newRefreshToken = refreshTokenService.GenerateToken();
        context.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = refreshTokenService.HashToken(newRefreshToken.Token),
            ExpiresAt = newRefreshToken.ExpiresAt,
            UserId = stored.UserId
        });
        await context.SaveChangesAsync(cancellationToken);

        var token = jwtTokenService.GenerateToken(stored.User);
        return new AuthResponse(token.Token, token.ExpiresAt, newRefreshToken.Token);
    }
}
