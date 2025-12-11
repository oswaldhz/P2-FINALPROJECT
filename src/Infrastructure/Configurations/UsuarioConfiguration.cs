using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Rol).IsRequired().HasMaxLength(50);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
