using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> entity)
    {
        entity.HasKey(p => p.Id);

        entity.Property(p => p.Amount)
            .HasPrecision(10, 2);

        entity.Property(p => p.TransactionRef)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(p => p.Status)
            .IsRequired();

        entity.HasOne(p => p.Booking)
            .WithMany()
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}