using Application.DTOs.Request;
using Application.Interfaces;
using Application.Services;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Application Services
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IVehicleServices, VehicleServices>(); // <-- ADD THIS LINE
        services.AddScoped<IPaymentService, PaymentService>();

        // Register Validators
        services.AddScoped<IValidator<CreateBookingRequest>, CreateBookingRequestValidator>();
       
        return services;
    }
}