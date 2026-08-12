using System.Security.Cryptography;
using System.Text;
using Application.DTOs;
using Application.DTOs.Request;
using Application.Services.Interfaces;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponse> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required.");

        var existingUser = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (existingUser != null)
            throw new InvalidOperationException(
                "A user with this email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            PasswordHash = HashPassword(request.Password),
            IsVerified = false
        };

        await _userRepository.AddAsync(user, cancellationToken);

        return new AuthResponse(
            GenerateToken(user),
            user.Id,
            user.Email,
            user.Role.ToString());
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user == null)
            throw new InvalidOperationException(
                "Invalid email or password.");

        var passwordHash = HashPassword(request.Password);

        if (user.PasswordHash != passwordHash)
            throw new InvalidOperationException(
                "Invalid email or password.");

        return new AuthResponse(
            GenerateToken(user),
            user.Id,
            user.Email,
            user.Role.ToString());
    }

    public async Task<UserResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (user == null)
            return null;

        return new UserResponse(
            user.Id,
            user.Name,
            string.Empty,
            user.Email,
            user.Role.ToString());
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }

    private static string GenerateToken(User user)
    {
        return Convert.ToBase64String(
            Encoding.UTF8.GetBytes(
                $"{user.Id}:{user.Email}:{Guid.NewGuid()}"));
    }
}