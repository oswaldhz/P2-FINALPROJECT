using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquiposController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EquiposController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var equipos = await _context.Equipos
            .Include(e => e.Softwares)
            .Select(e => new
            {
                e.Id,
                e.Nombre,
                e.Descripcion,
                Softwares = e.Softwares.Select(s => new { s.Id, s.Nombre, s.Version })
            })
            .ToListAsync();

        return Ok(equipos);
    }
}
