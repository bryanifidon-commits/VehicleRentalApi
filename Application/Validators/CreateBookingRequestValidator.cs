using Application.DTOs.Request;
using FluentValidation;

namespace Application.Validators;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle ID is required.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be strictly after the start date.");
    }
}