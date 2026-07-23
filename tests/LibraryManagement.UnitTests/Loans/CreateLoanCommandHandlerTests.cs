using FluentAssertions;
using LibraryManagement.Application.Loans.Commands.CreateLoan;
using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UnitTests.Loans;

public class CreateLoanCommandHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_Create_Loan_And_Decrement_AvailableCopies()
    {
        // Arrange
        await using var context = CreateContext();
        var book = new Book { Id = Guid.NewGuid(), Title = "Book", Isbn = "1234567890", PublicationYear = 2020, TotalCopies = 2, AvailableCopies = 2, AuthorId = Guid.NewGuid() };
        var member = new Member { Id = Guid.NewGuid(), FullName = "Member", Email = "m@example.com", MembershipDate = DateTime.UtcNow, IsActive = true };
        await context.Books.AddAsync(book);
        await context.Members.AddAsync(member);
        await context.SaveChangesAsync();

        var handler = new CreateLoanCommandHandler(new BookRepository(context), new MemberRepository(context), new LoanRepository(context), new UnitOfWork(context));
        var dto = new CreateLoanDto { BookId = book.Id, MemberId = member.Id };

        // Act
        var result = await handler.Handle(new CreateLoanCommand(dto), CancellationToken.None);

        // Assert
        result.BookId.Should().Be(book.Id);
        book.AvailableCopies.Should().Be(1);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Book_Not_Available()
    {
        // Arrange
        await using var context = CreateContext();
        var book = new Book { Id = Guid.NewGuid(), Title = "Book", Isbn = "1234567890", PublicationYear = 2020, TotalCopies = 1, AvailableCopies = 0, AuthorId = Guid.NewGuid() };
        var member = new Member { Id = Guid.NewGuid(), FullName = "Member", Email = "m@example.com", MembershipDate = DateTime.UtcNow, IsActive = true };
        await context.Books.AddAsync(book);
        await context.Members.AddAsync(member);
        await context.SaveChangesAsync();

        var handler = new CreateLoanCommandHandler(new BookRepository(context), new MemberRepository(context), new LoanRepository(context), new UnitOfWork(context));
        var dto = new CreateLoanDto { BookId = book.Id, MemberId = member.Id };

        // Act
        var act = async () => await handler.Handle(new CreateLoanCommand(dto), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*not available*");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Member_Is_Inactive()
    {
        // Arrange
        await using var context = CreateContext();
        var book = new Book { Id = Guid.NewGuid(), Title = "Book", Isbn = "1234567890", PublicationYear = 2020, TotalCopies = 1, AvailableCopies = 1, AuthorId = Guid.NewGuid() };
        var member = new Member { Id = Guid.NewGuid(), FullName = "Member", Email = "m@example.com", MembershipDate = DateTime.UtcNow, IsActive = false };
        await context.Books.AddAsync(book);
        await context.Members.AddAsync(member);
        await context.SaveChangesAsync();

        var handler = new CreateLoanCommandHandler(new BookRepository(context), new MemberRepository(context), new LoanRepository(context), new UnitOfWork(context));
        var dto = new CreateLoanDto { BookId = book.Id, MemberId = member.Id };

        // Act
        var act = async () => await handler.Handle(new CreateLoanCommand(dto), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*not active*");
    }
}
