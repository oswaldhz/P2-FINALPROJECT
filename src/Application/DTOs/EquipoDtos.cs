namespace Application.DTOs;

public record SoftwareDto(Guid Id, string Nombre, string Version);

public record EquipoDto(
    Guid Id,
    string Nombre,
    string? Descripcion,
    IEnumerable<SoftwareDto> Softwares
);
