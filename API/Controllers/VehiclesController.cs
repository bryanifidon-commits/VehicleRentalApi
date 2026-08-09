using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Application.DTOs.RequestDtos;

namespace VehicleRentalApi.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAvailableVehicles(CancellationToken cancellationToken)
        {
            var response = await _vehicleService.GetAvailableVehiclesAsync(cancellationToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetVehicleById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _vehicleService.GetVehicleByIdAsync(id, cancellationToken);

            if (response == null)
            {
                return NotFound(new { message = $"Vehicle with ID '{id}' was not found." });
            }

            return Ok(response);
        }

        //[Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request, CancellationToken cancellationToken)
        {
            var response = await _vehicleService.CreateVehicleAsync(request, cancellationToken);
            return Ok(response);
        }

        //[Authorize(Policy = "AdminOnly")]
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateVehicleStatus(Guid id, [FromBody] UpdateVehicleStatusRequest request, CancellationToken cancellationToken)
        {
            await _vehicleService.UpdateVehicleStatusAsync(id, request, cancellationToken);
            return NoContent();
        }
    }
}