using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Common;

public record TokenResult(string Token, DateTime ExpiresAt);

public interface IJwtTokenService
{
    TokenResult GenerateToken(User user);
}