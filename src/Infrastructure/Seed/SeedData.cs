using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Data.ApplicationDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        await db.Database.MigrateAsync();

        if (!db.Usuarios.Any())
        {
            var adminConfig = GetAdminUserConfiguration(configuration, environment);
            var hasher = new PasswordHasher<Usuario>();
            var admin = new Usuario
            {
                Id = Guid.NewGuid(),
                Nombre = adminConfig.Name,
                Email = adminConfig.Email,
                Rol = "Admin"
            };
            admin.PasswordHash = hasher.HashPassword(admin, adminConfig.Password);
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

    private static AdminUserConfiguration GetAdminUserConfiguration(IConfiguration configuration, IHostEnvironment environment)
    {
        var adminSection = configuration.GetSection("AdminUser");
        var email = adminSection["Email"];
        var name = adminSection["Name"];
        var password = adminSection["Password"];

        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(password))
        {
            return new AdminUserConfiguration(name, email, password);
        }

        if (environment.IsDevelopment())
        {
            return new AdminUserConfiguration("Administrador", "admin@example.com", "Admin123$");
        }

        throw new InvalidOperationException("Admin user settings (AdminUser:Email, AdminUser:Name, AdminUser:Password) must be provided in configuration or environment variables.");
    }

    private sealed record AdminUserConfiguration(string Name, string Email, string Password);
}
