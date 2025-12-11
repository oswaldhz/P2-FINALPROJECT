using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservasController : ControllerBase
{
    private readonly IReservaService _reservaService;

    public ReservasController(IReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservaDto>>> GetAll()
    {
        var reservas = await _reservaService.GetAllAsync();
        return Ok(reservas);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservaDto>> GetById(Guid id)
    {
        var reserva = await _reservaService.GetByIdAsync(id);

        if (reserva is null)
        {
            return NotFound();
        }

        return Ok(reserva);
    }

    [HttpPost]
    public async Task<ActionResult<ReservaDto>> Create(ReservaCreateDto request)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(userId, out var usuarioId))
        {
            return Unauthorized();
        }

        var resultado = await _reservaService.CreateAsync(usuarioId, request);
        if (!resultado.Success || resultado.Value is null)
        {
            return BadRequest(new { message = resultado.Error });
        }

        return CreatedAtAction(nameof(GetById), new { id = resultado.Value.Id }, resultado.Value);
    }

    [HttpPut("{id:guid}/estado")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEstado(Guid id, [FromBody] string estado)
    {
        var resultado = await _reservaService.UpdateEstadoAsync(id, estado);
        if (!resultado.Success)
        {
            return NotFound(new { message = resultado.Error });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var resultado = await _reservaService.DeleteAsync(id);
        if (!resultado.Success)
        {
            return NotFound(new { message = resultado.Error });
        }

        return NoContent();
    }
}
