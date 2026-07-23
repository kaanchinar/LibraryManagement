using FluentAssertions;
using LibraryManagement.Application.Auth;
using LibraryManagement.Application.Auth.Commands.Register;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LibraryManagement.UnitTests.Auth;

public class RegisterCommandHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static AuthTokenIssuer CreateTokenIssuer(AppDbContext context)
    {
        var jwtService = new Mock<IJwtTokenService>();
        jwtService.Setup(s => s.GenerateToken(It.IsAny<User>()))
            .Returns(new TokenResult("test-access-token", DateTime.UtcNow.AddMinutes(15)));

        var refreshService = new Mock<IRefreshTokenService>();
        refreshService.Setup(s => s.GenerateToken())
            .Returns(new RefreshTokenResult("test-refresh-token", DateTime.UtcNow.AddDays(7)));
        refreshService.Setup(s => s.HashToken(It.IsAny<string>()))
            .Returns((string t) => $"hash-{t}");

        return new AuthTokenIssuer(jwtService.Object, refreshService.Object,
            new RefreshTokenRepository(context), new UnitOfWork(context));
    }

    [Fact]
    public async Task Handle_Should_Create_User_And_Member_With_Default_Role()
    {
        // Arrange
        await using var context = CreateContext();
        var handler = new RegisterCommandHandler(new UserRepository(context), new MemberRepository(context), CreateTokenIssuer(context));
        var command = new RegisterCommand("New Member", "new@example.com", "Password123");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Token.Should().Be("test-access-token");
        result.RefreshToken.Should().Be("test-refresh-token");

        var user = await context.Users.Include(u => u.Member).SingleAsync();
        user.Email.Should().Be(command.Email);
        user.Role.Should().Be(Role.Member);
        BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash).Should().BeTrue();

        user.Member.Should().NotBeNull();
        user.Member!.FullName.Should().Be(command.FullName);
        user.Member.Email.Should().Be(command.Email);
        user.Member.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Email_Taken()
    {
        // Arrange
        await using var context = CreateContext();
        context.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Email = "taken@example.com",
            PasswordHash = "hash",
            Role = Role.Member
        });
        await context.SaveChangesAsync();

        var handler = new RegisterCommandHandler(new UserRepository(context), new MemberRepository(context), CreateTokenIssuer(context));
        var command = new RegisterCommand("Someone", "taken@example.com", "Password123");

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Member_Email_Taken()
    {
        // Arrange
        await using var context = CreateContext();
        context.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            FullName = "Existing Member",
            Email = "member@example.com",
            MembershipDate = DateTime.UtcNow,
            IsActive = true
        });
        await context.SaveChangesAsync();

        var handler = new RegisterCommandHandler(new UserRepository(context), new MemberRepository(context), CreateTokenIssuer(context));
        var command = new RegisterCommand("Someone", "member@example.com", "Password123");

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>();
    }
}
