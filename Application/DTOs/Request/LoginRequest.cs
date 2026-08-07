namespace Application.DTOs.RequestDtos;

public record LoginRequest(
    string Email,
    string Password
);