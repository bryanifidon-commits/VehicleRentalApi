using Application.DTOs.RequestDtos;
using Application.DTOs.ResponseDtos;

namespace Application.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default);
    Task<BookingResponse?> GetBookingByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingResponse>> GetUserBookingsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingResponse>> GetAllBookingsAsync(CancellationToken cancellationToken = default);
    Task CancelBookingAsync(Guid bookingId, Guid userId, CancellationToken cancellationToken = default);
}