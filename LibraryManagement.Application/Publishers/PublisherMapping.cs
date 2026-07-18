using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Publishers;

public static class PublisherMapping
{
    public static PublisherDto ToDto(this Publisher publisher)
    {
        return new PublisherDto
        {
            Id = publisher.Id,
            Name = publisher.Name,
            CreatedAt = publisher.CreatedAt
        };
    }

    public static Publisher ToEntity(this CreatePublisherDto dto)
    {
        return new Publisher
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };
    }

    public static void UpdateEntity(this UpdatePublisherDto dto, Publisher publisher)
    {
        publisher.Name = dto.Name;
    }
}
