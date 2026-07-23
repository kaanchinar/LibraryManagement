namespace LibraryManagement.Application.Auth.Dtos;

public record RegisterRequest(string FullName, string Email, string Password);
