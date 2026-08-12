using Domain.Enums;

namespace Application.DTOs.Request;

public record UpdateVehicleStatusRequest(
    VehicleStatus Status
);