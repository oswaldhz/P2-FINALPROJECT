using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Models;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReservasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservaResponse>>> GetAll()
    {
        var reservas = await _context.Reservas
            .Include(r => r.Equipo)
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.FechaInicio)
            .Select(r => new ReservaResponse(
                r.Id,
                r.EquipoId,
                r.Equipo!.Nombre,
                r.UsuarioId,
                r.Usuario!.Nombre,
                r.FechaInicio,
                r.FechaFin,
                r.Estado,
                r.Notas))
            .ToListAsync();

        return reservas;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservaResponse>> GetById(Guid id)
    {
        var reserva = await _context.Reservas
            .Include(r => r.Equipo)
            .Include(r => r.Usuario)
            .Where(r => r.Id == id)
            .Select(r => new ReservaResponse(
                r.Id,
                r.EquipoId,
                r.Equipo!.Nombre,
                r.UsuarioId,
                r.Usuario!.Nombre,
                r.FechaInicio,
                r.FechaFin,
                r.Estado,
                r.Notas))
            .FirstOrDefaultAsync();

        if (reserva is null)
        {
            return NotFound();
        }

        return reserva;
    }

    [HttpPost]
    public async Task<ActionResult<ReservaResponse>> Create(ReservaCreateRequest request)
    {
        if (request.FechaFin <= request.FechaInicio)
        {
            return BadRequest(new { message = "La fecha de fin debe ser mayor a la de inicio" });
        }

        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(userId, out var usuarioId))
        {
            return Unauthorized();
        }

        var equipoExists = await _context.Equipos.AnyAsync(e => e.Id == request.EquipoId);
        if (!equipoExists)
        {
            return NotFound(new { message = "Equipo no encontrado" });
        }

        var reserva = new Reserva
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            EquipoId = request.EquipoId,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            Notas = request.Notas,
            Estado = "Pendiente"
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = reserva.Id }, new ReservaResponse(
            reserva.Id,
            reserva.EquipoId,
            (await _context.Equipos.FindAsync(reserva.EquipoId))?.Nombre ?? string.Empty,
            reserva.UsuarioId,
            (await _context.Usuarios.FindAsync(reserva.UsuarioId))?.Nombre ?? string.Empty,
            reserva.FechaInicio,
            reserva.FechaFin,
            reserva.Estado,
            reserva.Notas));
    }

    [HttpPut("{id:guid}/estado")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEstado(Guid id, [FromBody] string estado)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva is null)
        {
            return NotFound();
        }

        reserva.Estado = estado;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva is null)
        {
            return NotFound();
        }

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
