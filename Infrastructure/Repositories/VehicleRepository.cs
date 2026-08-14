using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VehicleRentalDbContext _context;

    public VehicleRepository(VehicleRentalDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> GetAvailableAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .Where(v => v.Status == VehicleStatus.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Vehicle vehicle,
        CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Vehicle vehicle,
        CancellationToken cancellationToken = default)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _context.Vehicles.FindAsync(new object[] { id }, cancellationToken);
        if (vehicle != null)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}