using FluentAssertions;
using LibraryManagement.Application.Authors.Commands.CreateAuthor;
using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UnitTests.Authors;

public class CreateAuthorCommandHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_Create_Author()
    {
        // Arrange
        await using var context = CreateContext();
        var handler = new CreateAuthorCommandHandler(context);
        var dto = new CreateAuthorDto { Name = "Test Author", Bio = "Bio" };

        // Act
        var result = await handler.Handle(new CreateAuthorCommand(dto), CancellationToken.None);

        // Assert
        result.Name.Should().Be(dto.Name);
        result.Bio.Should().Be(dto.Bio);
        (await context.Authors.CountAsync()).Should().Be(1);
    }
}
