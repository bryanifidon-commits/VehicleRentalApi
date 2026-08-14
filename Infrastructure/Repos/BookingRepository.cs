using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repos;

public class BookingRepository : IBookingRepository
{
    private static readonly List<Booking> _bookings = new();

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(booking);
    }

    public Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Booking>>(_bookings);
    }

    public Task<IEnumerable<Booking>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var customerBookings = _bookings.Where(b => b.CustomerId == customerId);
        return Task.FromResult<IEnumerable<Booking>>(customerBookings);
    }

    public Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return GetByCustomerIdAsync(userId, cancellationToken);
    }

    public Task<IEnumerable<Booking>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var vehicleBookings = _bookings.Where(b => b.VehicleId == vehicleId);
        return Task.FromResult<IEnumerable<Booking>>(vehicleBookings);
    }

    public Task<IEnumerable<Booking>> GetActiveBookingsForVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var activeBookings = _bookings.Where(b => b.VehicleId == vehicleId && b.Status != BookingStatus.Cancelled);
        return Task.FromResult<IEnumerable<Booking>>(activeBookings);
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _bookings.Add(booking);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        var index = _bookings.FindIndex(b => b.Id == booking.Id);
        if (index != -1)
        {
            _bookings[index] = booking;
        }
        return Task.CompletedTask;
    }
}