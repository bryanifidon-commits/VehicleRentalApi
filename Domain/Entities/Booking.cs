using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Booking
    {
    public Guid Id { get; set; } = Guid.NewGuid();
    
    
    public Guid VehicleId { get; set; }
    public Guid CustomerId { get; set; }

   
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.PendingPayment;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties for EF Core
    public Vehicle Vehicle { get; set; } = null!;
    public User Customer { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}