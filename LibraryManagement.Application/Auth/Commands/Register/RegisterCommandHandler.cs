using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Application.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository users,
    IMemberRepository members,
    AuthTokenIssuer tokenIssuer)
    : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var emailTaken = await users.EmailExistsAsync(request.Email, cancellationToken)
            || await members.EmailExistsAsync(request.Email, cancellationToken);

        if (emailTaken)
        {
            throw new BusinessRuleException("Email is already registered.");
        }

        var member = new Member
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            MembershipDate = DateTime.UtcNow,
            IsActive = true
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Role.Member,
            Member = member
        };

        await users.AddAsync(user, cancellationToken);

        return await tokenIssuer.IssueAsync(user, cancellationToken);
    }
}
