namespace Domain.Entities;

public class Reserva
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid EquipoId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public string? Notas { get; set; }

    public Usuario? Usuario { get; set; }
    public Equipo? Equipo { get; set; }
}
