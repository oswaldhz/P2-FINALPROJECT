using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservaRepository : IReservaRepository
{
    private readonly ApplicationDbContext _context;

    public ReservaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Reserva reserva)
    {
        await _context.Reservas.AddAsync(reserva);
    }

    public async Task<Reserva?> FindByIdAsync(Guid id)
    {
        return await _context.Reservas.FindAsync(id);
    }

    public async Task<List<Reserva>> GetAllWithDetailsAsync()
    {
        return await _context.Reservas
            .Include(r => r.Equipo)
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.FechaInicio)
            .ToListAsync();
    }

    public async Task<Reserva?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.Reservas
            .Include(r => r.Equipo)
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> HasOverlapAsync(Guid equipoId, DateTime inicio, DateTime fin)
    {
        return await _context.Reservas.AnyAsync(r => r.EquipoId == equipoId &&
            ((inicio >= r.FechaInicio && inicio < r.FechaFin) ||
             (fin > r.FechaInicio && fin <= r.FechaFin) ||
             (inicio <= r.FechaInicio && fin >= r.FechaFin)));
    }

    public void Remove(Reserva reserva)
    {
        _context.Reservas.Remove(reserva);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
