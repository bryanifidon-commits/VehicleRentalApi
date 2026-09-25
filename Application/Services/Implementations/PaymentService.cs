namespace Application.Services.Implementations;

using Application.DTOs;
using Application.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;

public class PaymentService : IPaymentService
{
    private readonly IPaystackService _paystackService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IConfiguration _configuration;

    public PaymentService(
        IPaystackService paystackService,
        IPaymentRepository paymentRepository,
        IBookingRepository bookingRepository,
        IConfiguration configuration)
    {
        _paystackService = paystackService;
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
        _configuration = configuration;
    }

    public async Task<InitializePaymentResponse> InitializePaymentAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking == null)
            throw new KeyNotFoundException($"Booking with ID {bookingId} was not found.");

        if (booking.Status != BookingStatus.PendingPayment)
            throw new InvalidOperationException("Booking must be in PendingPayment status to initialize payment.");

        string reference = $"VRS-{booking.Id}-{Guid.NewGuid().ToString("N")[..8]}";

        // Dynamically pull CallbackUrl from configuration with a local fallback
        string callbackUrl = _configuration["Paystack:CallbackUrl"]
            ?? "http://localhost:5228/swagger/index.html";

        string customerEmail = booking.Customer?.Email ?? "customer@example.com";

        var paystackResponse = await _paystackService.InitializeTransactionAsync(
            customerEmail,
            booking.TotalPrice,
            reference,
            callbackUrl
        );

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Amount = booking.TotalPrice,
            Status = PaymentStatus.Failed, // Default state until verified
            TransactionRef = reference,
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment, cancellationToken);

        return new InitializePaymentResponse(
            paystackResponse.Data.AuthorizationUrl,
            reference,
            "Payment initialized successfully."
        );
    }

    public async Task<VerifyPaymentResponse> VerifyPaymentAsync(string reference, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByTransactionRefAsync(reference, cancellationToken);
        if (payment == null)
        {
            return new VerifyPaymentResponse(
                false,
                "Payment transaction reference not found.",
                Guid.Empty,
                "NotFound"
            );
        }

        var verifyResponse = await _paystackService.VerifyTransactionAsync(reference);

        if (verifyResponse.Status && verifyResponse.Data.Status == "success")
        {
            payment.Status = PaymentStatus.Succeeded;

            var booking = await _bookingRepository.GetByIdAsync(payment.BookingId, cancellationToken);
            if (booking != null)
            {
                booking.Status = BookingStatus.Confirmed;
                await _bookingRepository.UpdateAsync(booking, cancellationToken);
            }

            await _paymentRepository.UpdateAsync(payment, cancellationToken);

            return new VerifyPaymentResponse(
                true,
                "Payment verified successfully. Booking confirmed!",
                payment.BookingId,
                BookingStatus.Confirmed.ToString()
            );
        }

        return new VerifyPaymentResponse(
            false,
            "Payment verification failed on Paystack.",
            payment.BookingId,
            payment.Status.ToString()
        );
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = request.BookingId,
            Amount = request.Amount,
            Status = PaymentStatus.Succeeded,
            TransactionRef = request.TransactionRef ?? $"MANUAL-{Guid.NewGuid().ToString("N")[..8]}",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment, cancellationToken);

        return new PaymentResponse(
            payment.Id,
            payment.BookingId,
            payment.Amount,
            payment.TransactionRef,
            payment.Status.ToString(),
            payment.CreatedAt
        );
    }

    public async Task<PaymentResponse?> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (payment == null) return null;

        return new PaymentResponse(
            payment.Id,
            payment.BookingId,
            payment.Amount,
            payment.TransactionRef,
            payment.Status.ToString(),
            payment.CreatedAt
        );
    }

    public async Task<PaymentResponse?> GetPaymentByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByBookingIdAsync(bookingId, cancellationToken);
        if (payment == null) return null;

        return new PaymentResponse(
            payment.Id,
            payment.BookingId,
            payment.Amount,
            payment.TransactionRef,
            payment.Status.ToString(),
            payment.CreatedAt
        );
    }
}