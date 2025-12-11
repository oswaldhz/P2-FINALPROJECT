namespace Domain.Entities;

public class Equipo
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    public ICollection<Software> Softwares { get; set; } = new List<Software>();
}
