using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        // Common operations (Authentication & Profile)
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);

        // Admin monitoring operation
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    }
    }
