namespace Domain.Entities;

public class Software
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}
