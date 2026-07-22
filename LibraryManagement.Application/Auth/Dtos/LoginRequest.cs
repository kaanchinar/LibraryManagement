namespace LibraryManagement.Application.Auth.Dtos;

public record LoginRequest(string Email, string Password);

public record AuthResponse(string Token, DateTime ExpiresAt);