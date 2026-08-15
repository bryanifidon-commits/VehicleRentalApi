namespace API.Controllers;

using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await _paymentService.ProcessPaymentAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetPaymentById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPaymentById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _paymentService.GetPaymentByIdAsync(id, cancellationToken);
        if (response == null) return NotFound();

        return Ok(response);
    }

    [HttpGet("booking/{bookingId:guid}")]
    public async Task<IActionResult> GetPaymentByBookingId(Guid bookingId, CancellationToken cancellationToken)
    {
        var response = await _paymentService.GetPaymentByBookingIdAsync(bookingId, cancellationToken);
        if (response == null) return NotFound();

        return Ok(response);
    }
}