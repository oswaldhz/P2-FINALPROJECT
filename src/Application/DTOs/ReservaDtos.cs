using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record ReservaCreateDto(
    [property: Required] Guid EquipoId,
    [property: Required] DateTime FechaInicio,
    [property: Required] DateTime FechaFin,
    string? Notas
);

public record ReservaDto(
    Guid Id,
    Guid EquipoId,
    string Equipo,
    Guid UsuarioId,
    string Usuario,
    DateTime FechaInicio,
    DateTime FechaFin,
    string Estado,
    string? Notas
);
