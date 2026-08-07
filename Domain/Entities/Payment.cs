using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Payment
    {
   public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string TransactionRef { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Booking Booking { get; set; } = null!;
    }
}