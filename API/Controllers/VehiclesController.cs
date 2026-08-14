using Application.DTOs.Request;
using Application.DTOs.RequestDtos;
using Application.DTOs.Response;
using Application.Services.Interfaces;
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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVehicles(CancellationToken cancellationToken)
    {
        var response = await _vehicleService.GetAllVehiclesAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicleById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _vehicleService.GetVehicleByIdAsync(id, cancellationToken);
        if (response == null)
            return NotFound(new { message = $"Vehicle with ID '{id}' was not found." });

        return Ok(response);
    }


    [HttpPost]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVehicle(Guid id, CancellationToken cancellationToken)
    {
        var success = await _vehicleService.DeleteVehicleAsync(id, cancellationToken);
        if (!success)
            return NotFound(new { message = $"Vehicle with ID '{id}' was not found." });

        return NoContent();
    }
}