namespace LibraryManagement.Application.Auth.Dtos;

public record LoginRequest(string Email, string Password);

public record RefreshRequest(string RefreshToken);

public record AuthResponse(string Token, DateTime ExpiresAt, string RefreshToken);
