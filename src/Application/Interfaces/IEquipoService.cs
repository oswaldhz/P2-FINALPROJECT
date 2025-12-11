using Application.DTOs;

namespace Application.Interfaces;

public interface IEquipoService
{
    Task<IEnumerable<EquipoDto>> GetAllAsync();
}
