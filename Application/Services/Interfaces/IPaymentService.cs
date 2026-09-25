namespace Application.Services.Interfaces;

using Application.DTOs;

public interface IPaymentService
{
    Task<PaymentResponse> ProcessPaymentAsync(CreatePaymentRequest request, CancellationToken cancellationToken);
    Task<PaymentResponse?> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PaymentResponse?> GetPaymentByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken);

    // Paystack integration methods
    Task<InitializePaymentResponse> InitializePaymentAsync(Guid bookingId, CancellationToken cancellationToken);
    Task<VerifyPaymentResponse> VerifyPaymentAsync(string reference, CancellationToken cancellationToken);
}