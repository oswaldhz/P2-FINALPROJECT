using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EquipoConfiguration : IEntityTypeConfiguration<Equipo>
{
    public void Configure(EntityTypeBuilder<Equipo> builder)
    {
        builder.ToTable("Equipos");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Descripcion).HasMaxLength(500);

        builder
            .HasMany(e => e.Softwares)
            .WithMany(s => s.Equipos)
            .UsingEntity<Dictionary<string, object>>(
                "EquipoSoftware",
                j => j
                    .HasOne<Software>()
                    .WithMany()
                    .HasForeignKey("SoftwareId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Equipo>()
                    .WithMany()
                    .HasForeignKey("EquipoId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("EquipoId", "SoftwareId");
                    j.ToTable("EquiposSoftwares");
                });
    }
}
