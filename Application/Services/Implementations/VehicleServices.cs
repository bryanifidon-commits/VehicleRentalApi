using Application.DTOs;
using Application.DTOs.Request;
using Application.DTOs.RequestDtos;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Request;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Implementations;

public class VehicleServices : IVehicleServices
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleServices(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync(CancellationToken cancellationToken = default)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);
        return vehicles.Select(MapToDTO);
    }

    public async Task<IEnumerable<VehicleDTO>> GetAvailableVehiclesAsync(CancellationToken cancellationToken = default)
    {
        var vehicles = await _vehicleRepository.GetAvailableAsync(cancellationToken);
        return vehicles.Select(MapToDTO);
    }

    public async Task<PagedResult<VehicleDTO>> SearchAndFilterVehiclesAsync(
        VehicleSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var (vehicles, totalCount) = await _vehicleRepository.GetFilteredAsync(request, cancellationToken);

        var dtos = vehicles.Select(MapToDTO);

        int pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        int pageSize = request.PageSize < 1 ? 10 : request.PageSize;
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<VehicleDTO>(
            dtos,
            totalCount,
            pageNumber,
            pageSize,
            totalPages
        );
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
            Status = VehicleStatus.Active,
            ImageUrl = request.ImageUrl
        };

        await _vehicleRepository.AddAsync(vehicle, cancellationToken);
        return MapToDTO(vehicle);
    }

    public async Task<VehicleDTO?> UpdateVehicleAsync(Guid id, UpdateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
        if (vehicle == null)
            return null;

        vehicle.Make = request.Make;
        vehicle.Model = request.Model;
        vehicle.Type = request.Type;
        vehicle.RegistrationNumber = request.RegistrationNumber;
        vehicle.Location = request.Location;
        vehicle.PricePerDay = request.PricePerDay;
        vehicle.ImageUrl = request.ImageUrl;

        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        return MapToDTO(vehicle);
    }

    public async Task<bool> UpdateVehicleStatusAsync(Guid id, UpdateVehicleStatusRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
        if (vehicle == null)
            return false;

        vehicle.Status = request.Status;
        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
        if (vehicle == null)
            return false;

        await _vehicleRepository.DeleteAsync(id, cancellationToken);
        return true;
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