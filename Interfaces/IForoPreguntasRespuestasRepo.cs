using System.Collections.Generic;
using BlogApi.Models;

namespace BlogApi.Interfaces
{
    public interface IForoPreguntasRespuestasRepo
    {
        IEnumerable<ForoPreguntasRespuesta> GetForoRespuesta();
        ForoPreguntasRespuesta GetForoRespuestaPorId(int Id);
        IEnumerable<ForoPreguntasRespuesta> GetForoRespuestaPorIdPregunta(int id);
        bool GuardarCambio(string usuario);   
        void CrearForoRespuesta(ForoPreguntasRespuesta respuesta);
        void ActualizaForoRespuesta(ForoPreguntasRespuesta respuesta);
        void EliminaForoRespuesta(ForoPreguntasRespuesta respuesta);
    }
}