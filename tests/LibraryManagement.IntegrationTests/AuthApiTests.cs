using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using LibraryManagement.Application.Auth.Dtos;
using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Common;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryManagement.IntegrationTests;

public class AuthApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Register_Should_Return_Token_That_Accesses_Protected_Endpoints()
    {
        // Arrange
        var request = new RegisterRequest("Integration User", $"user-{Guid.NewGuid()}@example.com", "Password123");

        // Act
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var auth = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        auth.Should().NotBeNull();
        auth!.Token.Should().NotBeNullOrEmpty();
        auth.RefreshToken.Should().NotBeNullOrEmpty();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
        var booksResponse = await _client.GetAsync("/api/books");
        booksResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_Should_Forbid_Admin_Only_Endpoints()
    {
        // Arrange
        var request = new RegisterRequest("Member User", $"member-{Guid.NewGuid()}@example.com", "Password123");
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", request);
        var auth = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        var books = await _client.GetFromJsonAsync<PagedResult<BookDto>>("/api/books");
        var bookId = books!.Items.First().Id;

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/books/{bookId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Register_Should_Return_400_When_Email_Taken()
    {
        // Arrange
        var email = $"dup-{Guid.NewGuid()}@example.com";
        var request = new RegisterRequest("First User", email, "Password123");
        await _client.PostAsJsonAsync("/api/auth/register", request);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("Second User", email, "Password123"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
