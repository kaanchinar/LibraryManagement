namespace LibraryManagement.Application.Publishers.Dtos;

public class PublisherDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
