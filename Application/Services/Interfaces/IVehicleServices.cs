using Application.DTOs.RequestDtos;
using Application.DTOs.Response;

namespace Application.Services.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<VehicleResponse>> GetAllVehiclesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleResponse>> GetAvailableVehiclesAsync(CancellationToken cancellationToken = default);
    Task<VehicleResponse?> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default);
    Task<VehicleResponse?> UpdateVehicleAsync(Guid id, UpdateVehicleRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateVehicleStatusAsync(Guid id, UpdateVehicleStatusRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default);
}