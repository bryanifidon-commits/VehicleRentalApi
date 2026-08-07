using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities
{


    public class Vehicle
    {
public Guid Id { get; set; } = Guid.NewGuid();
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;                 
    public string RegistrationNumber { get; set; } = string.Empty;    
    public string Location { get; set; } = string.Empty;             
    public decimal PricePerDay { get; set; }                           
    public VehicleStatus Status { get; set; } = VehicleStatus.Active;  
    public string? ImageUrl { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

