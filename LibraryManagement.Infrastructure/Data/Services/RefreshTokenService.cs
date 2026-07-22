using System.Security.Cryptography;
using LibraryManagement.Application.Common;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.Infrastructure.Data.Services;

public class RefreshTokenService(IConfiguration configuration) : IRefreshTokenService
{
    public RefreshTokenResult GenerateToken()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiresAt = DateTime.UtcNow.AddDays(configuration.GetValue("Jwt:RefreshExpiryDays", 7));
        return new RefreshTokenResult(token, expiresAt);
    }

    public string HashToken(string token)
    {
        var bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}
