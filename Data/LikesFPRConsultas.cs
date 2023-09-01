using BlogApi.Interfaces;
using BlogApi.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogApi.Data
{
    public class LikesFPRConsultas : ILikesPR
    {
        private readonly BlogHCContext _context;
        public LikesFPRConsultas(BlogHCContext context)
        {
            _context = context;
        }

        public void CrearLikeFP(ForoPreguntasLike preguntalike)
        {
            if(preguntalike == null)
                throw new ArgumentNullException(nameof(preguntalike));

            _context.ForoPreguntasLikes.Add(preguntalike);
        }

        public void CrearLikeFPR(ForoPreguntasRespuestasLike respuestaLike)
        {
            if(respuestaLike == null)
                throw new ArgumentNullException(nameof(respuestaLike));

            _context.ForoPreguntasRespuestasLikes.Add(respuestaLike);
        }

        public void EliminaLikeFP(ForoPreguntasLike preguntalike)
        {
            if(preguntalike == null)
                throw new ArgumentNullException(nameof(preguntalike));

            _context.Remove(preguntalike);
        }

        public void EliminaLikeFPR(ForoPreguntasRespuestasLike respuestaLike)
        {
            if(respuestaLike == null)
                throw new ArgumentNullException(nameof(respuestaLike));

            _context.Remove(respuestaLike);
        }

        public ForoPreguntasLike ExisteLikePregunta(string Usuario, int Id_Pregunta)
        {
            return _context.ForoPreguntasLikes.Where(x => x.IdUsuarioFlp == Usuario && x.IdPreguntaFpl == Id_Pregunta).FirstOrDefault();
        }

        public ForoPreguntasRespuestasLike ExisteLikeRespuesta(string Usuario, int Id_Respuesta)
        {
            return _context.ForoPreguntasRespuestasLikes.Where(x => x.IdUsuarioFlrp == Usuario && x.IdRespuestaFprl == Id_Respuesta).FirstOrDefault();
        }

        public bool GuardarCambio()
        {
            return _context.SaveChanges() > 0;
        }
    }
}