using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public BookingService(
        IBookingRepository bookingRepository,
        IVehicleRepository vehicleRepository)
    {
        _bookingRepository = bookingRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<BookingResponse> CreateBookingAsync(
        CreateBookingRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.StartDate < DateTime.UtcNow.Date)
            throw new InvalidOperationException("Start date cannot be in the past.");

        if (request.EndDate <= request.StartDate)
            throw new InvalidOperationException("End date must be after start date.");

        var vehicle = await _vehicleRepository.GetByIdAsync(
            request.VehicleId,
            cancellationToken);

        if (vehicle == null)
            throw new InvalidOperationException(
                $"Vehicle with ID '{request.VehicleId}' was not found.");

        var existingBookings =
            await _bookingRepository.GetByVehicleIdAsync(
                request.VehicleId,
                cancellationToken);

        bool isOverlapping = existingBookings.Any(b =>
            b.Status != BookingStatus.Cancelled &&
            request.StartDate < b.EndDate &&
            request.EndDate > b.StartDate);

        if (isOverlapping)
            throw new InvalidOperationException(
                "The vehicle is already booked for the selected dates.");

        int totalDays =
            (int)Math.Ceiling(
                (request.EndDate - request.StartDate).TotalDays);

        if (totalDays <= 0)
            totalDays = 1;

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            VehicleId = request.VehicleId,
            CustomerId = request.CustomerId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalPrice = totalDays * vehicle.PricePerDay,
            Status = BookingStatus.PendingPayment,
            CreatedAt = DateTime.UtcNow
        };

        await _bookingRepository.AddAsync(
            booking,
            cancellationToken);

        return MapToResponse(booking);
    }

    public async Task<BookingResponse?> GetBookingByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(
            id,
            cancellationToken);

        return booking == null ? null : MapToResponse(booking);
    }

    public async Task<IEnumerable<BookingResponse>> GetCustomerBookingsAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetByCustomerIdAsync(
            customerId,
            cancellationToken);

        return bookings.Select(MapToResponse);
    }

    public async Task CancelBookingAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (booking == null)
            throw new InvalidOperationException(
                $"Booking with ID '{id}' was not found.");

        if (booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException(
                "Booking is already cancelled.");

        booking.Status = BookingStatus.Cancelled;

        await _bookingRepository.UpdateAsync(
            booking,
            cancellationToken);
    }

    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse(
            booking.Id,
            booking.VehicleId,
            booking.CustomerId,
            booking.StartDate,
            booking.EndDate,
            booking.TotalPrice,
            booking.Status);
    }
}