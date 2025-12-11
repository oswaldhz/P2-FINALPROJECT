using Domain.Entities;

namespace Api.Services;

public interface ITokenService
{
    string CreateToken(Usuario user);
}
