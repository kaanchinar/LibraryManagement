using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Loans;

public static class LoanMapping
{
    public static LoanDto ToDto(this Loan loan)
    {
        return new LoanDto
        {
            Id = loan.Id,
            BookId = loan.BookId,
            BookTitle = loan.Book.Title,
            MemberId = loan.MemberId,
            MemberName = loan.Member.FullName,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate,
            IsReturned = loan.IsReturned,
            IsOverdue = !loan.IsReturned && loan.DueDate < DateTime.UtcNow,
            CreatedAt = loan.CreatedAt
        };
    }

    public static Loan ToEntity(this CreateLoanDto dto, Book book, Member member)
    {
        var now = DateTime.UtcNow;

        return new Loan
        {
            Id = Guid.NewGuid(),
            BookId = dto.BookId,
            Book = book,
            MemberId = dto.MemberId,
            Member = member,
            LoanDate = now,
            DueDate = now.AddDays(14),
            IsReturned = false
        };
    }
}
