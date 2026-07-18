using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.Property(m => m.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(m => m.MembershipDate)
            .IsRequired();

        builder.Property(m => m.IsActive)
            .IsRequired();
    }
}
