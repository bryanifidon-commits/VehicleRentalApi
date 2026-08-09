using Application.DTOs;
using Application.Request;
namespace Application.Services.Interfaces;
using Application.DTOs.RequestDtos;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDTO>> GetAvailableVehiclesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleDTO>> SearchVehiclesAsync(VehicleSearchRequest request, CancellationToken cancellationToken = default);
    Task<VehicleDTO?> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleDTO> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default);
    Task UpdateVehicleStatusAsync(Guid vehicleId, UpdateVehicleStatusRequest request, CancellationToken cancellationToken = default);
}