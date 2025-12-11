using Api.Models;
using Api.Services;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public AuthController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var exists = await _context.Usuarios.AnyAsync(u => u.Email == request.Email);
        if (exists)
        {
            return Conflict(new { message = "Ya existe un usuario con ese email" });
        }

        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Nombre = request.Nombre,
            Rol = "User"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();

        var token = _tokenService.CreateToken(user);
        return new AuthResponse(token, user.Email, user.Nombre, user.Rol);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        var token = _tokenService.CreateToken(user);
        return new AuthResponse(token, user.Email, user.Nombre, user.Rol);
    }
}
