using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Application.Publishers.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryManagement.IntegrationTests;

public class DeleteWithDependenciesApiTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;

    public DeleteWithDependenciesApiTests(CustomWebApplicationFactory factory)
    {
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
    public async Task DeleteAuthor_With_Books_Should_Return_400()
    {
        var authors = await _client.GetFromJsonAsync<PagedResult<AuthorDto>>("/api/authors");
        var authorWithBooks = authors!.Items.First(a => a.Name == "George Orwell");

        var response = await _client.DeleteAsync($"/api/authors/{authorWithBooks.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteGenre_With_Books_Should_Return_400()
    {
        var genres = await _client.GetFromJsonAsync<PagedResult<GenreDto>>("/api/genres");
        var genreWithBooks = genres!.Items.First(g => g.Name == "Dystopian");

        var response = await _client.DeleteAsync($"/api/genres/{genreWithBooks.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeletePublisher_With_Books_Should_Return_400()
    {
        var publishers = await _client.GetFromJsonAsync<PagedResult<PublisherDto>>("/api/publishers");
        var publisherWithBooks = publishers!.Items.First(p => p.Name == "Penguin Books");

        var response = await _client.DeleteAsync($"/api/publishers/{publisherWithBooks.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteBook_With_Loans_Should_Return_400()
    {
        var bookId = await CreateLoanAndGetBookId();

        var response = await _client.DeleteAsync($"/api/books/{bookId}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteMember_With_Loans_Should_Return_400()
    {
        var memberId = await CreateLoanAndGetMemberId();

        var response = await _client.DeleteAsync($"/api/members/{memberId}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteAuthor_Without_Books_Should_Return_204()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/authors",
            new CreateAuthorDto { Name = "No Books Author", Bio = "Bio" });
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<AuthorDto>();

        var response = await _client.DeleteAsync($"/api/authors/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<Guid> CreateLoanAndGetBookId()
    {
        var (bookId, _) = await CreateLoan();
        return bookId;
    }

    private async Task<Guid> CreateLoanAndGetMemberId()
    {
        var (_, memberId) = await CreateLoan();
        return memberId;
    }

    private async Task<(Guid BookId, Guid MemberId)> CreateLoan()
    {
        var books = await _client.GetFromJsonAsync<PagedResult<BookDto>>("/api/books");
        var book = books!.Items.First(b => b.AvailableCopies > 0);
        var members = await _client.GetFromJsonAsync<PagedResult<MemberDto>>("/api/members");
        var member = members!.Items.First();

        var response = await _client.PostAsJsonAsync("/api/loans",
            new CreateLoanDto { BookId = book.Id, MemberId = member.Id });
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        return (book.Id, member.Id);
    }
}
