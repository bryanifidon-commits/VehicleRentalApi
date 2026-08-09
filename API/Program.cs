using Application.Interfaces;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Infrastructure.Repos;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS setup for frontend developers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Dependency Injection - Repositories
builder.Services.AddScoped<IVehicleRepository, InMemoryVehicleRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>(); // Update class name if using InMemoryBookingRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();       // Update class name if using InMemoryUserRepository

// Dependency Injection - Application Services
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IBookingService, BookingService>();
//builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Enable Swagger in all environments for testing
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();