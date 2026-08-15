namespace Application.Services;

using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = request.BookingId,
            Amount = request.Amount,
            Status = PaymentStatus.Succeeded,
            TransactionRef = string.IsNullOrWhiteSpace(request.TransactionRef)
                ? $"TXN-{Guid.NewGuid().ToString()[..8].ToUpper()}"
                : request.TransactionRef,
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment, cancellationToken);

        return MapToResponse(payment);
    }

    public async Task<PaymentResponse?> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        return payment == null ? null : MapToResponse(payment);
    }

    public async Task<PaymentResponse?> GetPaymentByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByBookingIdAsync(bookingId, cancellationToken);
        return payment == null ? null : MapToResponse(payment);
    }

    private static PaymentResponse MapToResponse(Payment payment)
    {
        return new PaymentResponse(
            payment.Id,
            payment.BookingId,
            payment.Amount,
            payment.Status.ToString(),
            payment.TransactionRef,
            payment.CreatedAt
        );
    }
}