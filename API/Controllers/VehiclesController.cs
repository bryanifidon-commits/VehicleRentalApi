using Application.DTOs;
using Application.DTOs.Request;
using Application.DTOs.RequestDtos;
using Application.DTOs.Response;
using Application.Request;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleServices _vehicleService;

    public VehiclesController(IVehicleServices vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // Public: Anyone can browse or search vehicles
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<VehicleDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVehicles(
        [FromQuery] VehicleSearchRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _vehicleService.SearchAndFilterVehiclesAsync(request, cancellationToken);
        return Ok(response);
    }

    // Public: Anyone can view a specific vehicle's details
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehicleDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicleById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _vehicleService.GetVehicleByIdAsync(id, cancellationToken);
        if (response == null)
            return NotFound(new { message = $"Vehicle with ID '{id}' was not found." });

        return Ok(response);
    }

    // Restricted: Only Admins can add new vehicles
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VehicleDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _vehicleService.CreateVehicleAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetVehicleById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Restricted: Only Admins can update vehicle details
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VehicleDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] UpdateVehicleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _vehicleService.UpdateVehicleAsync(id, request, cancellationToken);
            if (response == null)
                return NotFound(new { message = $"Vehicle with ID '{id}' was not found." });

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Restricted: Only Admins can remove vehicles
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteVehicle(Guid id, CancellationToken cancellationToken)
    {
        var success = await _vehicleService.DeleteVehicleAsync(id, cancellationToken);
        if (!success)
            return NotFound(new { message = $"Vehicle with ID '{id}' was not found." });

        return NoContent();
    }
}