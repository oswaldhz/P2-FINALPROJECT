using Domain.Entities;

namespace Application.Interfaces;

public interface IUsuarioRepository
{
    Task<bool> ExistsAsync(Guid id);
    Task<Usuario?> FindByIdAsync(Guid id);
}
