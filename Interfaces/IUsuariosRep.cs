using System.Collections.Generic;
using BlogApi.Models;

namespace BlogApi.Interfaces
{
    public interface IUsuariosRep
    {
        IEnumerable<Usuario> GetUsuarios();
        Usuario GetUsuarioPorId(string Id);
        bool GuardarCambio(string usuario);
        bool CrearUsuario(Usuario User);
        void ActualizaUsuario(Usuario User);
        void EliminaUsuario(Usuario User);
        bool RecuperarContra(string id, out string Id_Usuario);
        string[] ValidacionClaveRecupera(string clave, string usuario);
    }
}