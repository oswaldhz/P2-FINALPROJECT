using Domain.Entities;

namespace Application.Interfaces;

public interface IReservaRepository
{
    Task<List<Reserva>> GetAllWithDetailsAsync();
    Task<Reserva?> GetByIdWithDetailsAsync(Guid id);
    Task<Reserva?> FindByIdAsync(Guid id);
    Task<bool> HasOverlapAsync(Guid equipoId, DateTime inicio, DateTime fin);
    Task AddAsync(Reserva reserva);
    void Remove(Reserva reserva);
    Task<int> SaveChangesAsync();
}
