using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> entity)
    {
        entity.HasKey(v => v.Id);

        entity.Property(v => v.Make)
            .IsRequired();

        entity.Property(v => v.Model)
            .IsRequired();

        entity.Property(v => v.Type)
            .IsRequired();

        entity.Property(v => v.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(50);

        entity.HasIndex(v => v.RegistrationNumber)
            .IsUnique();

        entity.Property(v => v.Location)
            .IsRequired()
            .HasMaxLength(150);

        entity.Property(v => v.PricePerDay)
            .HasPrecision(10, 2);

        entity.Property(v => v.Status)
            .IsRequired();

        entity.Property(v => v.ImageUrl)
            .HasMaxLength(500);
    }
}