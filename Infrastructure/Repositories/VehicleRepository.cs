using Application.DTOs.Request;
using Application.Interfaces;
using Application.Request;
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

    public async Task<(IEnumerable<Vehicle> Items, int TotalCount)> GetFilteredAsync(
        VehicleSearchRequest filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Vehicles.AsNoTracking().AsQueryable();

        // Acceptance Criteria: Return strictly Active vehicles
        query = query.Where(v => v.Status == VehicleStatus.Active);

        if (!string.IsNullOrWhiteSpace(filter.Make))
        {
            query = query.Where(v => EF.Functions.ILike(v.Make, $"%{filter.Make.Trim()}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.Model))
        {
            query = query.Where(v => EF.Functions.ILike(v.Model, $"%{filter.Model.Trim()}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.Type))
        {
            query = query.Where(v => EF.Functions.ILike(v.Type, $"%{filter.Type.Trim()}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.Location))
        {
            query = query.Where(v => EF.Functions.ILike(v.Location, $"%{filter.Location.Trim()}%"));
        }

        if (filter.MaxPricePerDay.HasValue)
        {
            query = query.Where(v => v.PricePerDay <= filter.MaxPricePerDay.Value);
        }

        int totalCount = await query.CountAsync(cancellationToken);

        int pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
        int pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

        var items = await query
            .OrderBy(v => v.Make)
            .ThenBy(v => v.Model)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.AsNoTracking().Where(v => v.Status == VehicleStatus.Active).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _context.Vehicles.FindAsync(new object[] { id }, cancellationToken);
        if (vehicle != null)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}