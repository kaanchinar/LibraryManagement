using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Common;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryManagement.IntegrationTests;

public class AuthorsApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthorsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task CreateAuthor_Should_Return_201_And_Location()
    {
        // Arrange
        var dto = new CreateAuthorDto { Name = "Integration Test Author", Bio = "Bio" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/authors", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<AuthorDto>();
        result.Should().NotBeNull();
        result!.Name.Should().Be(dto.Name);
    }

    [Fact]
    public async Task GetAuthors_Should_Return_PagedResult()
    {
        // Act
        var response = await _client.GetAsync("/api/authors");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<AuthorDto>>();
        result.Should().NotBeNull();
        result!.Items.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAuthor_With_Invalid_Data_Should_Return_400()
    {
        // Arrange
        var dto = new CreateAuthorDto { Name = "", Bio = "" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/authors", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
