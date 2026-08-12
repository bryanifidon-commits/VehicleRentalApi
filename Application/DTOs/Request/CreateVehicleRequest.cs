namespace Application.DTOs.Request;

public record CreateVehicleRequest(
    string Make,
    string Model,
    string Type,
    string RegistrationNumber,
    string Location,
    decimal PricePerDay,
    string? ImageUrl = null
);