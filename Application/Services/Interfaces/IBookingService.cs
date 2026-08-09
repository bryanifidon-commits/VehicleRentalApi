using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default);
    Task<BookingResponse?> GetBookingByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingResponse>> GetCustomerBookingsAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task CancelBookingAsync(Guid id, CancellationToken cancellationToken = default);
}