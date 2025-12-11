using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> builder)
    {
        builder.ToTable("Reservas");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Estado).IsRequired().HasMaxLength(50);
        builder.Property(r => r.Notas).HasMaxLength(1000);
        builder.Property(r => r.FechaInicio).IsRequired();
        builder.Property(r => r.FechaFin).IsRequired();

        builder
            .HasOne(r => r.Usuario)
            .WithMany(u => u.Reservas)
            .HasForeignKey(r => r.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.Equipo)
            .WithMany(e => e.Reservas)
            .HasForeignKey(r => r.EquipoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
