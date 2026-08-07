using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IReservationRepository
    {
 Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetActiveBookingsForVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default);
    
    // Customer operation
    Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    // Admin operation
    Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
}
    }
