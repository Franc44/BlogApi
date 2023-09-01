using BlogApi.DTOs;

namespace BlogApi.Interfaces
{
    public interface ISesionRep
    {
        UsuarioLeeDto Autenticacion(string usuario, string contrasena);
        UsuarioLeeDto RefreshToken(string usuario, string Token);
    }
}