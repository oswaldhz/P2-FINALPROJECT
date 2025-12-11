using Domain.Entities;

namespace Application.Interfaces;

public interface IEquipoRepository
{
    Task<List<Equipo>> GetAllWithSoftwareAsync();
    Task<bool> ExistsAsync(Guid id);
}
