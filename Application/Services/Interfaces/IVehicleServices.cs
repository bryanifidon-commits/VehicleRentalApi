using Application.DTOs.RequestDtos;
using Application.DTOs.ResponseDtos;

namespace Application.Services.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<VehicleResponse>> GetAvailableVehiclesAsync(CancellationToken cancellationToken = default);
    Task<VehicleResponse?> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default);
    Task UpdateVehicleStatusAsync(Guid vehicleId, UpdateVehicleStatusRequest request, CancellationToken cancellationToken = default);
}