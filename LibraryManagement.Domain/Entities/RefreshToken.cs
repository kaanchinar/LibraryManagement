using LibraryManagement.Domain.Common;

namespace LibraryManagement.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;
}
