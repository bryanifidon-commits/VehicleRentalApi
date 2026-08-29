namespace Application.DTOs;

public record UserResponse(
    Guid Id,
    string Name,
    string Phone,
    string Email,
    string Role
);