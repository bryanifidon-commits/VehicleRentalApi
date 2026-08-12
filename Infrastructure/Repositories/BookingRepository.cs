using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly VehicleRentalDbContext _context;

    public BookingRepository(VehicleRentalDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetActiveBookingsForVehicleAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Where(b =>
                b.VehicleId == vehicleId &&
                b.Status != BookingStatus.Cancelled)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Where(b => b.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Where(b => b.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Booking booking,
        CancellationToken cancellationToken = default)
    {
        await _context.Bookings.AddAsync(
            booking,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Booking booking,
        CancellationToken cancellationToken = default)
    {
        _context.Bookings.Update(booking);

        await _context.SaveChangesAsync(cancellationToken);
    }
}