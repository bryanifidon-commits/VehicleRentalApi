namespace Application.DTOs.Request;

public record CreateBookingRequest(
    Guid VehicleId,
    Guid CustomerId,
    DateTime StartDate,
    DateTime EndDate
);