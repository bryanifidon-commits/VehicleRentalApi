namespace Application.DTOs.Request;

public record RegisterUserRequest(
    string Name,
    string Email,
    string Phone,
    string Password
);