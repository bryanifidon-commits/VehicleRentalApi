namespace Application.DTOs
{
    public class VehicleDTO
    {
        public Guid Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
    }
}