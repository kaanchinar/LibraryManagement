using FluentAssertions;
using LibraryManagement.Application.Members.Commands.DeleteMember;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UnitTests.Members;

public class DeleteMemberCommandHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Member_Does_Not_Exist()
    {
        await using var context = CreateContext();
        var handler = new DeleteMemberCommandHandler(context);
        var nonExistentId = Guid.NewGuid();

        var act = () => handler.Handle(new DeleteMemberCommand(nonExistentId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Delete_Member_When_No_Loans_Exist()
    {
        await using var context = CreateContext();
        var member = new Member
        {
            FullName = "Member To Delete",
            Email = "delete@test.com",
            MembershipDate = DateTime.UtcNow,
            IsActive = true
        };
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var handler = new DeleteMemberCommandHandler(context);
        await handler.Handle(new DeleteMemberCommand(member.Id), CancellationToken.None);

        (await context.Members.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Throw_BusinessRuleException_When_Member_Has_Loans()
    {
        await using var context = CreateContext();

        var author = new Author { Name = "Author" };
        context.Authors.Add(author);

        var member = new Member
        {
            FullName = "Member With Loans",
            Email = "loans@test.com",
            MembershipDate = DateTime.UtcNow,
            IsActive = true
        };
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var book = new Book
        {
            Title = "Some Book",
            Isbn = "1234567890123",
            PublicationYear = 2024,
            TotalCopies = 1,
            AvailableCopies = 0,
            AuthorId = author.Id
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var loan = new Loan
        {
            BookId = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            IsReturned = false
        };
        context.Loans.Add(loan);
        await context.SaveChangesAsync();

        var handler = new DeleteMemberCommandHandler(context);
        var act = () => handler.Handle(new DeleteMemberCommand(member.Id), CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*loans*");
        (await context.Members.CountAsync()).Should().Be(1);
    }
}
