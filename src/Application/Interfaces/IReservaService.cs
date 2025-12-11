using Application.Common;
using Application.DTOs;

namespace Application.Interfaces;

public interface IReservaService
{
    Task<IEnumerable<ReservaDto>> GetAllAsync();
    Task<ReservaDto?> GetByIdAsync(Guid id);
    Task<Result<ReservaDto>> CreateAsync(Guid usuarioId, ReservaCreateDto request);
    Task<Result> UpdateEstadoAsync(Guid id, string estado);
    Task<Result> DeleteAsync(Guid id);
}
