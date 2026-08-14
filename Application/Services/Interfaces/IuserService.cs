using Application.DTOs;
using Application.DTOs.Request;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<AuthResponse> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default);

    Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);

    Task<UserResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}