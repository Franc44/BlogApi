using BlogApi.Models;

namespace BlogApi.Interfaces
{
    public interface ILikesPR
    {
        ForoPreguntasLike ExisteLikePregunta(string Usuario, int Id_Pregunta);
        ForoPreguntasRespuestasLike ExisteLikeRespuesta(string Usuario, int Id_Respuesta);
        bool GuardarCambio();   
        void CrearLikeFP(ForoPreguntasLike preguntalike);
        void EliminaLikeFP(ForoPreguntasLike preguntalike);
        void CrearLikeFPR(ForoPreguntasRespuestasLike respuestaLike);
        void EliminaLikeFPR(ForoPreguntasRespuestasLike respuestaLike);
    }
}