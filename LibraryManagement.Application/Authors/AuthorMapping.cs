using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Authors;

public static class AuthorMapping
{
    public static AuthorDto ToDto(this Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            CreatedAt = author.CreatedAt
        };
    }

    public static Author ToEntity(this CreateAuthorDto dto)
    {
        return new Author
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Bio = dto.Bio
        };
    }

    public static void UpdateEntity(this UpdateAuthorDto dto, Author author)
    {
        author.Name = dto.Name;
        author.Bio = dto.Bio;
    }
}
