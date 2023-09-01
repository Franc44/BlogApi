using System.Collections.Generic;
using BlogApi.Models;
using BlogApi.DTOs;

namespace BlogApi.Interfaces
{
    public interface IForoPreguntasRepo
    {
        IEnumerable<ForoPregunta> GetForoPregunta();
        ForoPregunta GetForoPreguntaPorId(int Id);
        IEnumerable<ForoPregunta> GetForoPreguntaPorTag(string tag);
        IEnumerable<ForoPreguntaLeeDto> GetForoPreguntasPorBusqueda(string buscaString, string usuario);
        IEnumerable<ForoPreguntaLeeDto> GetForoPreguntasPorUsuario(string usuario);
        
        string[] GetForoPreguntaTags();
        bool GuardarCambio(string usuario);   
        void CrearForoPregunta(ForoPregunta pregunta);
        void ActualizaForoPregunta(ForoPregunta pregunta);
        void EliminaForoPregunta(ForoPregunta pregunta);
    }
}