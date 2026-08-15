namespace Application.Interfaces;

using Application.DTOs;

public interface IPaymentService
{
    Task<PaymentResponse> ProcessPaymentAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default);
    Task<PaymentResponse?> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaymentResponse?> GetPaymentByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default);
}