namespace Application.DTOs.RequestDtos;

public record CreateVehicleRequest(
    string Make,
    string Model,
    int Year,
    decimal DailyRate,
    string LicensePlate
);