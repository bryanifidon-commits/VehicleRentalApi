using Application.DTOs.Request;
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

        // Register Validators
        services.AddScoped<IValidator<CreateBookingRequest>, CreateBookingRequestValidator>();

        return services;
    }
}