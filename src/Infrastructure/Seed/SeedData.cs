using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Data.ApplicationDbContext>();

        await db.Database.MigrateAsync();

        if (!db.Usuarios.Any())
        {
            var hasher = new PasswordHasher<Usuario>();
            var admin = new Usuario
            {
                Id = Guid.NewGuid(),
                Nombre = "Administrador",
                Email = "admin@example.com",
                Rol = "Admin"
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123$");
            db.Usuarios.Add(admin);
        }

        if (!db.Equipos.Any())
        {
            var equipo = new Equipo
            {
                Id = Guid.NewGuid(),
                Nombre = "Equipo de Diseño",
                Descripcion = "Estación con software gráfico"
            };
            var software = new Software
            {
                Id = Guid.NewGuid(),
                Nombre = "Figma",
                Version = "1.0"
            };
            equipo.Softwares.Add(software);

            db.Equipos.Add(equipo);
        }

        await db.SaveChangesAsync();
    }
}
