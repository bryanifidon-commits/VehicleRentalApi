using Domain.Enums;

namespace Application.DTOs.Response;

public record BookingResponse(
    Guid Id,
    Guid VehicleId,
    Guid CustomerId,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalPrice,
    BookingStatus Status
);