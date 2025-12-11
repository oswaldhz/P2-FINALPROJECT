using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IEquipoRepository _equipoRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public ReservaService(
        IReservaRepository reservaRepository,
        IEquipoRepository equipoRepository,
        IUsuarioRepository usuarioRepository)
    {
        _reservaRepository = reservaRepository;
        _equipoRepository = equipoRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<IEnumerable<ReservaDto>> GetAllAsync()
    {
        var reservas = await _reservaRepository.GetAllWithDetailsAsync();
        return reservas.Select(MapToDto);
    }

    public async Task<ReservaDto?> GetByIdAsync(Guid id)
    {
        var reserva = await _reservaRepository.GetByIdWithDetailsAsync(id);
        return reserva is null ? null : MapToDto(reserva);
    }

    public async Task<Result<ReservaDto>> CreateAsync(Guid usuarioId, ReservaCreateDto request)
    {
        if (request.FechaFin <= request.FechaInicio)
        {
            return Result<ReservaDto>.Fail("La fecha de fin debe ser mayor a la de inicio");
        }

        var usuarioExiste = await _usuarioRepository.ExistsAsync(usuarioId);
        if (!usuarioExiste)
        {
            return Result<ReservaDto>.Fail("Usuario no encontrado");
        }

        var equipoExiste = await _equipoRepository.ExistsAsync(request.EquipoId);
        if (!equipoExiste)
        {
            return Result<ReservaDto>.Fail("Equipo no encontrado");
        }

        var overlaps = await _reservaRepository.HasOverlapAsync(request.EquipoId, request.FechaInicio, request.FechaFin);
        if (overlaps)
        {
            return Result<ReservaDto>.Fail("Ya existe una reserva para el equipo en el rango solicitado");
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

        await _reservaRepository.AddAsync(reserva);
        await _reservaRepository.SaveChangesAsync();

        var creada = await _reservaRepository.GetByIdWithDetailsAsync(reserva.Id);
        if (creada is null)
        {
            return Result<ReservaDto>.Fail("No se pudo recuperar la reserva creada");
        }

        return Result<ReservaDto>.Ok(MapToDto(creada));
    }

    public async Task<Result> UpdateEstadoAsync(Guid id, string estado)
    {
        var reserva = await _reservaRepository.FindByIdAsync(id);
        if (reserva is null)
        {
            return Result.Fail("Reserva no encontrada");
        }

        reserva.Estado = estado;
        await _reservaRepository.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var reserva = await _reservaRepository.FindByIdAsync(id);
        if (reserva is null)
        {
            return Result.Fail("Reserva no encontrada");
        }

        _reservaRepository.Remove(reserva);
        await _reservaRepository.SaveChangesAsync();
        return Result.Ok();
    }

    private static ReservaDto MapToDto(Reserva reserva)
    {
        return new ReservaDto(
            reserva.Id,
            reserva.EquipoId,
            reserva.Equipo?.Nombre ?? string.Empty,
            reserva.UsuarioId,
            reserva.Usuario?.Nombre ?? string.Empty,
            reserva.FechaInicio,
            reserva.FechaFin,
            reserva.Estado,
            reserva.Notas);
    }
}
