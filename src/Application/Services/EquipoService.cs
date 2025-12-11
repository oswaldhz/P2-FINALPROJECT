using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class EquipoService : IEquipoService
{
    private readonly IEquipoRepository _equipoRepository;

    public EquipoService(IEquipoRepository equipoRepository)
    {
        _equipoRepository = equipoRepository;
    }

    public async Task<IEnumerable<EquipoDto>> GetAllAsync()
    {
        var equipos = await _equipoRepository.GetAllWithSoftwareAsync();
        return equipos.Select(e => new EquipoDto(
            e.Id,
            e.Nombre,
            e.Descripcion,
            e.Softwares.Select(s => new SoftwareDto(s.Id, s.Nombre, s.Version))));
    }
}
