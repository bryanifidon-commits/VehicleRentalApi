using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Request;

namespace Application.Interfaces
{
    public interface IVehicleRepository
    {
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetAllAvailableAsync(CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Vehicle>> SearchAsync(VehicleSearchRequest request, CancellationToken cancellationToken = default);
   
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
}
    }
