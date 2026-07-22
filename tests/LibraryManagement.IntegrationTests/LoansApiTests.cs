using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.IntegrationTests;

public class LoansApiTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public LoansApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public async Task InitializeAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("admin@library.com", "Admin@123"));
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Token);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateLoan_Should_Return_201_When_Book_Available()
    {
        // Arrange
        var author = await SeedAuthorAsync();
        var book = await SeedBookAsync(author.Id);
        var member = await SeedMemberAsync();

        var dto = new CreateLoanDto { BookId = book.Id, MemberId = member.Id };

        // Act
        var response = await _client.PostAsJsonAsync("/api/loans", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<LoanDto>();
        result.Should().NotBeNull();
        result!.BookId.Should().Be(book.Id);
        result.MemberId.Should().Be(member.Id);
    }

    [Fact]
    public async Task ReturnLoan_Should_Return_204()
    {
        // Arrange
        var author = await SeedAuthorAsync();
        var book = await SeedBookAsync(author.Id);
        var member = await SeedMemberAsync();

        var createResponse = await _client.PostAsJsonAsync("/api/loans", new CreateLoanDto { BookId = book.Id, MemberId = member.Id });
        var loan = await createResponse.Content.ReadFromJsonAsync<LoanDto>();

        // Act
        var response = await _client.PostAsync($"/api/loans/{loan!.Id}/return", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<AuthorDto> SeedAuthorAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/authors", new { Name = "Test Author", Bio = "Bio" });
        return (await response.Content.ReadFromJsonAsync<AuthorDto>())!;
    }

    private async Task<BookDto> SeedBookAsync(Guid authorId)
    {
        var dto = new CreateBookDto
        {
            Title = "Test Book",
            Isbn = Guid.NewGuid().ToString()[..10],
            PublicationYear = 2020,
            TotalCopies = 2,
            AuthorId = authorId
        };
        var response = await _client.PostAsJsonAsync("/api/books", dto);
        return (await response.Content.ReadFromJsonAsync<BookDto>())!;
    }

    private async Task<MemberDto> SeedMemberAsync()
    {
        var dto = new CreateMemberDto
        {
            FullName = "Test Member",
            Email = $"{Guid.NewGuid()}@example.com",
            MembershipDate = DateTime.UtcNow,
            IsActive = true
        };
        var response = await _client.PostAsJsonAsync("/api/members", dto);
        return (await response.Content.ReadFromJsonAsync<MemberDto>())!;
    }
}
