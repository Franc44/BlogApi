using System.Collections.Generic;
using BlogApi.DTOs;
using BlogApi.Models;

namespace BlogApi.Interfaces
{
    public interface IPublicacionesRepo
    {
        IEnumerable<PublicacionLeeDto> GetPublicaciones();
        Publicacione GetPublicacionPorId(int Id);
        IEnumerable<PublicacionLeeDto> GetPublicacionPorTipo(int tipo);
        bool GuardarCambio(string usuario);   
        void CrearPublicacion(Publicacione publicacion);
        void ActualizaPublicacion(Publicacione publicacion);
        void EliminaPublicacion(Publicacione publicacion);
    }
}