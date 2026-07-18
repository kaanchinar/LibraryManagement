using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.LoanDate)
            .IsRequired();

        builder.Property(l => l.DueDate)
            .IsRequired();

        builder.Property(l => l.IsReturned)
            .IsRequired();

        builder.HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Member)
            .WithMany(m => m.Loans)
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.MemberId);
        builder.HasIndex(l => l.BookId);
        builder.HasIndex(l => l.IsReturned);
        builder.HasIndex(l => l.DueDate);
    }
}
