using Application.DTOs;
using Application.DTOs.RequestDtos;
using Application.Interfaces; // or Application.Repositories depending on where IVehicleRepository is located
using Application.Request;
using Application.Services.Interfaces;
using Domain.Entities;

namespace Application.Services.Implementations;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IEnumerable<VehicleDTO>> GetAvailableVehiclesAsync(CancellationToken cancellationToken = default)
    {
        var vehicles = await _vehicleRepository.GetAllAvailableAsync(cancellationToken);
        return vehicles.Select(MapToDTO);
    }

    public async Task<IEnumerable<VehicleDTO>> SearchVehiclesAsync(VehicleSearchRequest request, CancellationToken cancellationToken = default)
    {
        var vehicles = await _vehicleRepository.SearchAsync(request, cancellationToken);
        return vehicles.Select(MapToDTO);
    }

    public async Task<VehicleDTO?> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
        return vehicle == null ? null : MapToDTO(vehicle);
    }

    public async Task<VehicleDTO> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Make = request.Make,
            Model = request.Model,
            Type = request.Type,
            RegistrationNumber = request.RegistrationNumber,
            Location = request.Location,
            PricePerDay = request.PricePerDay,
            ImageUrl = request.ImageUrl
        };

        await _vehicleRepository.AddAsync(vehicle, cancellationToken);
        return MapToDTO(vehicle);
    }

    public async Task UpdateVehicleStatusAsync(Guid vehicleId, UpdateVehicleStatusRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle == null)
            throw new InvalidOperationException($"Vehicle with ID '{vehicleId}' was not found.");

        vehicle.Status = request.Status;
        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
    }

    private static VehicleDTO MapToDTO(Vehicle vehicle)
    {
        return new VehicleDTO(
            vehicle.Id,
            vehicle.Make,
            vehicle.Model,
            vehicle.Type,
            vehicle.RegistrationNumber,
            vehicle.Location,
            vehicle.PricePerDay,
            vehicle.Status,
            vehicle.ImageUrl
        );
    }
}