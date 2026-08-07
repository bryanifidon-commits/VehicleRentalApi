namespace Application.DTOs.RequestDtos;

public record CreateBookingRequest(
    Guid UserId,
    Guid VehicleId,
    DateTime StartDate,
    DateTime EndDate
);