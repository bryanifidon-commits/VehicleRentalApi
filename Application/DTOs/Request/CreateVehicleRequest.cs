namespace Application.DTOs.RequestDtos;

public record CreateVehicleRequest(
    string Make,
    string Model,
    string Type,
    string RegistrationNumber,
    string Location,
    decimal PricePerDay,
    string? ImageUrl = null
);