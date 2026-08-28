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
        ValidateBookingDates(request.StartDate, request.EndDate);

        // 1. Verify vehicle exists and is active
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken)
            ?? throw new InvalidOperationException($"Vehicle with ID '{request.VehicleId}' was not found.");

        if (vehicle.Status != VehicleStatus.Active)
            throw new InvalidOperationException("This vehicle is currently unavailable for rental.");

        // 2. Check for overlapping bookings
        await EnsureNoBookingOverlapAsync(request.VehicleId, request.StartDate, request.EndDate, cancellationToken: cancellationToken);

        // 3. Calculate total days and total price
        var totalDays = (int)Math.Ceiling((request.EndDate - request.StartDate).TotalDays);
        if (totalDays <= 0) totalDays = 1;

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

        await _bookingRepository.AddAsync(booking, cancellationToken);
        return MapToResponse(booking);
    }

    public async Task<BookingResponse?> GetBookingByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);
        return booking == null ? null : MapToResponse(booking);
    }

    public async Task<IEnumerable<BookingResponse>> GetCustomerBookingsAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        return bookings.Select(MapToResponse);
    }

    // Fetches all system bookings for Admin view
    public async Task<IEnumerable<BookingResponse>> GetAllBookingsAsync(
        CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetAllAsync(cancellationToken);
        return bookings.Select(MapToResponse);
    }

    public async Task<BookingResponse> UpdateBookingDatesAsync(
        Guid id,
        UpdateBookingDatesRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateBookingDates(request.StartDate, request.EndDate);

        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Booking with ID '{id}' was not found.");

        if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
            throw new InvalidOperationException("Cannot change dates for a cancelled or completed booking.");

        // Check overlap while ignoring this booking's own ID
        await EnsureNoBookingOverlapAsync(booking.VehicleId, request.StartDate, request.EndDate, excludeBookingId: id, cancellationToken);

        var vehicle = await _vehicleRepository.GetByIdAsync(booking.VehicleId, cancellationToken)
            ?? throw new InvalidOperationException("Associated vehicle was not found.");

        var totalDays = (int)Math.Ceiling((request.EndDate - request.StartDate).TotalDays);
        if (totalDays <= 0) totalDays = 1;

        booking.StartDate = request.StartDate;
        booking.EndDate = request.EndDate;
        booking.TotalPrice = totalDays * vehicle.PricePerDay;

        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        return MapToResponse(booking);
    }

    public async Task UpdateBookingStatusAsync(
        Guid id,
        BookingStatus newStatus,
        CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Booking with ID '{id}' was not found.");

        if (booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Cannot update status on a cancelled booking.");

        booking.Status = newStatus;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
    }

    public async Task CancelBookingAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await UpdateBookingStatusAsync(id, BookingStatus.Cancelled, cancellationToken);
    }

    // --- Private Helper Methods ---

    private static void ValidateBookingDates(DateTime start, DateTime end)
    {
        if (start < DateTime.UtcNow.Date)
            throw new InvalidOperationException("Start date cannot be in the past.");

        if (end <= start)
            throw new InvalidOperationException("End date must be strictly after the start date.");
    }

    private async Task EnsureNoBookingOverlapAsync(
        Guid vehicleId,
        DateTime start,
        DateTime end,
        Guid? excludeBookingId = null,
        CancellationToken cancellationToken = default)
    {
        var existingBookings = await _bookingRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);

        bool isOverlapping = existingBookings.Any(b =>
            b.Id != excludeBookingId &&
            b.Status != BookingStatus.Cancelled &&
            start < b.EndDate &&
            end > b.StartDate);

        if (isOverlapping)
            throw new InvalidOperationException("The vehicle is already booked for the selected date range.");
    }

    private static BookingResponse MapToResponse(Booking booking) =>
        new(
            booking.Id,
            booking.VehicleId,
            booking.CustomerId,
            booking.StartDate,
            booking.EndDate,
            booking.TotalPrice,
            booking.Status
        );
}