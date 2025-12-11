using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Usuarios.AnyAsync(u => u.Id == id);
    }

    public async Task<Usuario?> FindByIdAsync(Guid id)
    {
        return await _context.Usuarios.FindAsync(id);
    }
}
