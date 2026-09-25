namespace Application.DTOs;

public record InitializePaymentResponse(
    string AuthorizationUrl,
    string Reference,
    string Message
);

public record VerifyPaymentResponse(
    bool Success,
    string Message,
    Guid BookingId,
    string Status
);