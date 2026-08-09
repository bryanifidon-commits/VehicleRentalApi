using Domain.Enums;

namespace Application.DTOs;

public record VehicleDTO(
    Guid Id,
    string Make,
    string Model,
    string Type,
    string RegistrationNumber,
    string Location,
    decimal PricePerDay,
    VehicleStatus Status,
    string? ImageUrl
);