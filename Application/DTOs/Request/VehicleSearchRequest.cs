namespace Application.Request;

public record VehicleSearchRequest(
    string? SearchTerm = null,
    string? Make = null,
    string? Model = null,
    string? Type = null,
    string? Location = null,
    decimal? MinPricePerDay = null,
    decimal? MaxPricePerDay = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int PageNumber = 1,
    int PageSize = 10
);