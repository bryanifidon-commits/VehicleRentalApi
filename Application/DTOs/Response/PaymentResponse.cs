namespace Application.DTOs;

public record PaymentResponse(
    Guid Id,
    Guid BookingId,
    decimal Amount,
    string TransactionRef,
    string Status,
    DateTime CreatedAt
);