using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class SoftwareConfiguration : IEntityTypeConfiguration<Software>
{
    public void Configure(EntityTypeBuilder<Software> builder)
    {
        builder.ToTable("Softwares");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Version).IsRequired().HasMaxLength(50);
    }
}
