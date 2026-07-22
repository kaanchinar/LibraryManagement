using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Common;

public interface IAppDbContext
{
    DbSet<Author> Authors { get; }
    DbSet<Genre> Genres { get; }
    DbSet<Publisher> Publishers { get; }
    DbSet<Book> Books { get; }
    DbSet<Member> Members { get; }
    DbSet<Loan> Loans { get; }
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
