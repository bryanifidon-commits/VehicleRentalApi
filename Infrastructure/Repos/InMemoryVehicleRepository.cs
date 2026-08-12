using Application.Repositories;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repos;

public class InMemoryVehicleRepository : IVehicleRepository
{
    private readonly List<Vehicle> _vehicles;

    public InMemoryVehicleRepository()
    {
        // Pre-load data when the API starts
        _vehicles = new List<Vehicle>
        {
            new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "Toyota",
                Model = "Camry",
                Type = "Sedan",
                RegistrationNumber = "LAG-123-XY",
                Location = "Lagos Island",
                PricePerDay = 65.00m,
                Status = VehicleStatus.Active,
                ImageUrl = "https://example.com/camry.jpg"
            },
            new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "Honda",
                Model = "Civic",
                Type = "Sedan",
                RegistrationNumber = "ABJ-456-YZ",
                Location = "Ikeja",
                PricePerDay = 55.00m,
                Status = VehicleStatus.Active,
                ImageUrl = "https://example.com/civic.jpg"
            },
            new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "Ford",
                Model = "Explorer",
                Type = "SUV",
                RegistrationNumber = "PHC-789-ZA",
                Location = "Victoria Island",
                PricePerDay = 120.00m,
                Status = VehicleStatus.Maintenance,
                ImageUrl = "https://example.com/explorer.jpg"
            }
        };
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_vehicles);
    }

    public async Task<IEnumerable<Vehicle>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_vehicles.Where(v => v.Status == VehicleStatus.Active));
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_vehicles.FirstOrDefault(v => v.Id == id));
    }

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        _vehicles.Add(vehicle);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        var index = _vehicles.FindIndex(v => v.Id == vehicle.Id);
        if (index != -1)
        {
            _vehicles[index] = vehicle;
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = _vehicles.FirstOrDefault(v => v.Id == id);
        if (vehicle != null)
        {
            _vehicles.Remove(vehicle);
        }
        await Task.CompletedTask;
    }
}