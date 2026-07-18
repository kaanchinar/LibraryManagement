using FluentAssertions;
using LibraryManagement.Application.Loans.Commands.ReturnLoan;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UnitTests.Loans;

public class ReturnLoanCommandHandlerTests
{
    private static (DbContextOptions<AppDbContext> Options, AppDbContext SeedContext, AppDbContext HandlerContext) CreateContexts()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return (options, new AppDbContext(options), new AppDbContext(options));
    }

    [Fact]
    public async Task Handle_Should_Return_Loan_And_Increment_AvailableCopies()
    {
        // Arrange
        var (options, seedContext, handlerContext) = CreateContexts();
        await using (seedContext)
        await using (handlerContext)
        {
            var book = new Book { Id = Guid.NewGuid(), Title = "Book", Isbn = "1234567890", PublicationYear = 2020, TotalCopies = 1, AvailableCopies = 0, AuthorId = Guid.NewGuid() };
            var member = new Member { Id = Guid.NewGuid(), FullName = "Member", Email = "m@example.com", MembershipDate = DateTime.UtcNow, IsActive = true };
            var loan = new Loan { Id = Guid.NewGuid(), Book = book, Member = member, BookId = book.Id, MemberId = member.Id, LoanDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(14), IsReturned = false };
            await seedContext.Books.AddAsync(book);
            await seedContext.Members.AddAsync(member);
            await seedContext.Loans.AddAsync(loan);
            await seedContext.SaveChangesAsync();

            var handler = new ReturnLoanCommandHandler(handlerContext);

            // Act
            await handler.Handle(new ReturnLoanCommand(loan.Id), CancellationToken.None);

            // Assert
            var updatedLoan = await handlerContext.Loans.FindAsync(loan.Id);
            var updatedBook = await handlerContext.Books.FindAsync(book.Id);
            updatedLoan!.IsReturned.Should().BeTrue();
            updatedLoan.ReturnDate.Should().NotBeNull();
            updatedBook!.AvailableCopies.Should().Be(1);
        }
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Loan_Already_Returned()
    {
        // Arrange
        var (options, seedContext, handlerContext) = CreateContexts();
        await using (seedContext)
        await using (handlerContext)
        {
            var book = new Book { Id = Guid.NewGuid(), Title = "Book", Isbn = "1234567890", PublicationYear = 2020, TotalCopies = 1, AvailableCopies = 1, AuthorId = Guid.NewGuid() };
            var member = new Member { Id = Guid.NewGuid(), FullName = "Member", Email = "m@example.com", MembershipDate = DateTime.UtcNow, IsActive = true };
            var loan = new Loan { Id = Guid.NewGuid(), Book = book, Member = member, BookId = book.Id, MemberId = member.Id, LoanDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(14), IsReturned = true, ReturnDate = DateTime.UtcNow };
            await seedContext.Books.AddAsync(book);
            await seedContext.Members.AddAsync(member);
            await seedContext.Loans.AddAsync(loan);
            await seedContext.SaveChangesAsync();

            var handler = new ReturnLoanCommandHandler(handlerContext);

            // Act
            var act = async () => await handler.Handle(new ReturnLoanCommand(loan.Id), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*already been returned*");
        }
    }
}
