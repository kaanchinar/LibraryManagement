namespace LibraryManagement.Application.Common;

public record RefreshTokenResult(string Token, DateTime ExpiresAt);

public interface IRefreshTokenService
{
    RefreshTokenResult GenerateToken();
    string HashToken(string token);
}
