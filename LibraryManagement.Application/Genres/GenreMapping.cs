using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Genres;

public static class GenreMapping
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            CreatedAt = genre.CreatedAt
        };
    }

    public static Genre ToEntity(this CreateGenreDto dto)
    {
        return new Genre
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };
    }

    public static void UpdateEntity(this UpdateGenreDto dto, Genre genre)
    {
        genre.Name = dto.Name;
    }
}
