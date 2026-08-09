using Application.Interfaces;
using Application.Request;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repos;

public class InMemoryVehicleRepository : IVehicleRepository
{
    private static readonly List<Vehicle> _vehicles = new()
    {
        new Vehicle 
        { 
            Id = Guid.NewGuid(), 
            Make = "Toyota", 
            Model = "Camry", 
            Type = "Sedan", 
            RegistrationNumber = "ABC-123", 
            Location = "Lagos", 
            PricePerDay = 50.00m, 
            Status = VehicleStatus.Active 
        },
        new Vehicle 
        { 
            Id = Guid.NewGuid(), 
            Make = "Honda", 
            Model = "Civic", 
            Type = "Sedan", 
            RegistrationNumber = "DEF-456", 
            Location = "Abuja", 
            PricePerDay = 45.00m, 
            Status = VehicleStatus.Active 
        },
        new Vehicle 
        { 
            Id = Guid.NewGuid(), 
            Make = "Tesla", 
            Model = "Model 3", 
            Type = "Electric", 
            RegistrationNumber = "EV-789", 
            Location = "Lagos", 
            PricePerDay = 100.00m, 
            Status = VehicleStatus.Active 
        }
    };

    public Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_vehicles.FirstOrDefault(v => v.Id == id));

    public Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_vehicles.AsEnumerable());

    public Task<IEnumerable<Vehicle>> GetAllAvailableAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_vehicles.Where(v => v.Status == VehicleStatus.Active).AsEnumerable());

    public Task<IEnumerable<Vehicle>> SearchAsync(VehicleSearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _vehicles.Where(v => v.Status == VehicleStatus.Active).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Make))
            query = query.Where(v => v.Make.Contains(request.Make, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Model))
            query = query.Where(v => v.Model.Contains(request.Model, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Type))
            query = query.Where(v => v.Type.Contains(request.Type, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Location))
            query = query.Where(v => v.Location.Contains(request.Location, StringComparison.OrdinalIgnoreCase));

        if (request.MaxPricePerDay.HasValue)
            query = query.Where(v => v.PricePerDay <= request.MaxPricePerDay.Value);

        var results = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return Task.FromResult<IEnumerable<Vehicle>>(results);
    }

    public Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        _vehicles.Add(vehicle);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        var existing = _vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
        if (existing != null)
        {
            _vehicles.Remove(existing);
            _vehicles.Add(vehicle);
        }
        return Task.CompletedTask;
    }
}