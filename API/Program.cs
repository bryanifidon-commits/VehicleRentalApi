using Application.Interfaces;
using Application.Repositories;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using FluentValidation;
using Infrastructure.Repos;

var builder = WebApplication.CreateBuilder(args);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<VehicleServices>();

// Repositories
builder.Services.AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();

// Application Services
builder.Services.AddScoped<IVehicleServices, VehicleServices>();
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();