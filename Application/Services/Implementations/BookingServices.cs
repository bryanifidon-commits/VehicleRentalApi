using Application.DTOs.RequestDtos;
using Application.DTOs.ResponseDtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUserRepository _userRepository;

    public BookingService(
        IBookingRepository bookingRepository,
        IVehicleRepository vehicleRepository,
        IUserRepository userRepository)
    {
        _bookingRepository = bookingRepository;
        _vehicleRepository = vehicleRepository;
        _userRepository = userRepository;
    }

    public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Validate User Exists
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.UserId} does not exist.");
        }

        // 2. Validate Vehicle Exists
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle == null)
        {
            throw new InvalidOperationException($"Vehicle with ID {request.VehicleId} does not exist.");
        }

        // 3. Check for Overlapping Bookings
        var existingBookings = await _bookingRepository.GetActiveBookingsForVehicleAsync(request.VehicleId, cancellationToken);
        bool hasOverlap = existingBookings.Any(b =>
            (request.StartDate < b.EndDate) && (request.EndDate > b.StartDate));

        if (hasOverlap)
        {
            throw new InvalidOperationException("The vehicle is already booked for the selected date range.");
        }

        // 4. Calculate Total Price based on Duration
        int totalDays = (int)(request.EndDate - request.StartDate).TotalDays;
        if (totalDays <= 0)
        {
            throw new ArgumentException("End date must be after start date.");
        }
        decimal totalPrice = totalDays * vehicle.DailyRate;

        // 5. Create Entity
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            VehicleId = request.VehicleId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalPrice = totalPrice,
            Status = BookingStatus.PendingPayment
        };

        await _bookingRepository.AddAsync(booking, cancellationToken);

        // 6. Return Response DTO
        return new BookingResponse(
            booking.Id,
            booking.UserId,
            booking.VehicleId,
            booking.StartDate,
            booking.EndDate,
            booking.TotalPrice,
            booking.Status.ToString()
        );
    }
}