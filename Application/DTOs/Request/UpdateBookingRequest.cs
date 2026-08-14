namespace Application.DTOs.Request;

public record UpdateBookingDatesRequest(
    DateTime StartDate,
    DateTime EndDate
);