using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly VehicleRentalDbContext _context;

    public PaymentRepository(VehicleRentalDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);
    }

    public async Task<Payment?> GetByBookingIdAsync(
        Guid bookingId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(
                p => p.BookingId == bookingId,
                cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Payment payment,
        CancellationToken cancellationToken = default)
    {
        await _context.Payments.AddAsync(
            payment,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Payment payment,
        CancellationToken cancellationToken = default)
    {
        _context.Payments.Update(payment);

        await _context.SaveChangesAsync(cancellationToken);
    }
}