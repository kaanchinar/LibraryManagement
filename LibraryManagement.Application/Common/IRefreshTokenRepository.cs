using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Common;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashWithUserAsync(string hash, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}
