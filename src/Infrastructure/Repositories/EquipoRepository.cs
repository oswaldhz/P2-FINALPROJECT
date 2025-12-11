using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EquipoRepository : IEquipoRepository
{
    private readonly ApplicationDbContext _context;

    public EquipoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Equipos.AnyAsync(e => e.Id == id);
    }

    public async Task<List<Equipo>> GetAllWithSoftwareAsync()
    {
        return await _context.Equipos
            .Include(e => e.Softwares)
            .ToListAsync();
    }
}
