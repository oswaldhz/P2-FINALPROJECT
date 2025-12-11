using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public record ReservaCreateRequest(
    [property: Required] Guid EquipoId,
    [property: Required] DateTime FechaInicio,
    [property: Required] DateTime FechaFin,
    string? Notas
);

public record ReservaResponse(
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
