using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Enums;

namespace Application.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(
        CreateBookingRequest request,
        CancellationToken cancellationToken = default);

    Task<BookingResponse?> GetBookingByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<BookingResponse>> GetCustomerBookingsAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<BookingResponse>> GetAllBookingsAsync(
        CancellationToken cancellationToken = default);

    Task<BookingResponse> UpdateBookingDatesAsync(
        Guid id,
        UpdateBookingDatesRequest request,
        CancellationToken cancellationToken = default);

    Task UpdateBookingStatusAsync(
        Guid id,
        BookingStatus newStatus,
        CancellationToken cancellationToken = default);

    Task CancelBookingAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}