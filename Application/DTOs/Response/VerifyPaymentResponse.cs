namespace Application.DTOs.Response;

public record VerifyPaymentResponse(
    bool Success,
    string Message,
    Guid BookingId,
    string Status
);