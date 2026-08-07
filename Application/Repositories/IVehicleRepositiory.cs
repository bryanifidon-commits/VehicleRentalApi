using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IVehicleRepositiory
    {
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetAllAvailableAsync(CancellationToken cancellationToken = default);
    
    // Admin operations
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
}
    }
