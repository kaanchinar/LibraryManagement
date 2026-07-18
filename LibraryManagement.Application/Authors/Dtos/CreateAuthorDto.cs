namespace LibraryManagement.Application.Authors.Dtos;

public class CreateAuthorDto
{
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
}
