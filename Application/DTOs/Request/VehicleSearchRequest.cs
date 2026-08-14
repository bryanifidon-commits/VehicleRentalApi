namespace Application.Request;

public record VehicleSearchRequest(
    string? Make = null,
    string? Model = null,
    string? Type = null,
    string? Location = null,
    decimal? MaxPricePerDay = null,
    int PageNumber = 1,
    int PageSize = 10
);