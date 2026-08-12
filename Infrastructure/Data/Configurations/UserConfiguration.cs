using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(u => u.Id);

        entity.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(150);

        entity.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        entity.HasIndex(u => u.Email)
            .IsUnique();

        entity.Property(u => u.Phone)
            .IsRequired()
            .HasMaxLength(30);

        entity.HasIndex(u => u.Phone)
            .IsUnique();

        entity.Property(u => u.PasswordHash)
            .IsRequired();

        entity.Property(u => u.Role)
            .IsRequired();

        entity.Property(u => u.IsVerified)
            .IsRequired();
    }
}