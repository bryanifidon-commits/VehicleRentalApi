using Domain.Enums;

namespace Application.DTOs.Response;

public record VehicleResponse(
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