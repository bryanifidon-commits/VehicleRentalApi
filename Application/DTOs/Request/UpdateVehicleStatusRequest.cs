using Domain.Enums;

namespace Application.DTOs.RequestDtos;

public record UpdateVehicleStatusRequest(
    VehicleStatus Status
);