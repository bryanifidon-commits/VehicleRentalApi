namespace Application.DTOs;

public record CreatePaymentRequest(
    Guid BookingId,
    decimal Amount,
    string? TransactionRef = null
);