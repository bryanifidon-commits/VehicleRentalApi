using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Payment?> GetByBookingIdAsync(
        Guid bookingId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Payment>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Payment payment,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Payment payment,
        CancellationToken cancellationToken = default);
}