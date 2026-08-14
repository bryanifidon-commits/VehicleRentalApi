namespace Application.DTOs.Request;

public class UpdateVehicleRequest
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal PricePerDay { get; set; }
    public string? ImageUrl { get; set; }
}