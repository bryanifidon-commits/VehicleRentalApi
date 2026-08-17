using Application.DTOs;
using Application.DTOs.Request;
using Application.DTOs.RequestDtos;
using Application.DTOs.Response;
using Application.Request;

namespace Application.Services.Interfaces;

public interface IVehicleServices
{
    Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleDTO>> GetAvailableVehiclesAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<VehicleDTO>> SearchAndFilterVehiclesAsync(VehicleSearchRequest request, CancellationToken cancellationToken = default);
    Task<VehicleDTO?> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleDTO> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default);
    Task<VehicleDTO?> UpdateVehicleAsync(Guid id, UpdateVehicleRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateVehicleStatusAsync(Guid id, UpdateVehicleStatusRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default);
}