using Application.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("initialize/{bookingId:guid}")]
    public async Task<ActionResult<InitializePaymentResponse>> InitializePayment(
        Guid bookingId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _paymentService.InitializePaymentAsync(bookingId, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("verify/{reference}")]
    public async Task<ActionResult<VerifyPaymentResponse>> VerifyPayment(
        string reference,
        CancellationToken cancellationToken)
    {
        var result = await _paymentService.VerifyPaymentAsync(reference, cancellationToken);

        if (!result.Success && result.Status == "NotFound")
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id, cancellationToken);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    [HttpGet("booking/{bookingId:guid}")]
    public async Task<ActionResult<PaymentResponse>> GetByBookingId(
        Guid bookingId,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId, cancellationToken);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }
}