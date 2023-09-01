using System.Collections.Generic;
using BlogApi.Interfaces;
using BlogApi.Models;
using System;
using System.Linq;

namespace BlogApi.Data
{
    public class ForoRespuestaConsultas : IForoPreguntasRespuestasRepo
    {
        private readonly BlogHCContext _context;
        private readonly IAccionUsuario _accion;
        public ForoRespuestaConsultas(BlogHCContext context, IAccionUsuario accion)
        {
            _context = context;
            _accion = accion;
        }
        public void ActualizaForoRespuesta(ForoPreguntasRespuesta respuesta)
        {
            
        }

        public void CrearForoRespuesta(ForoPreguntasRespuesta respuesta)
        {
            if(respuesta == null)
                throw new ArgumentNullException(nameof(respuesta));

            _context.ForoPreguntasRespuestas.Add(respuesta);
        }

        public void EliminaForoRespuesta(ForoPreguntasRespuesta respuesta)
        {
            if(respuesta == null)
                throw new ArgumentNullException(nameof(respuesta));
            
            var respuestaslikes = _context.ForoPreguntasRespuestasLikes.Where(x => x.IdRespuestaFprl == respuesta.IdRespuesta);
            
            _context.RemoveRange(respuestaslikes);
            _context.Remove(respuesta);    
        }

        public IEnumerable<ForoPreguntasRespuesta> GetForoRespuesta()
        {
            return _context.ForoPreguntasRespuestas.Where(x => x.EstatusRespuesta != 0).ToList().OrderBy(x => x.FechaRespuesta);
        }

        public ForoPreguntasRespuesta GetForoRespuestaPorId(int Id)
        {
            return _context.ForoPreguntasRespuestas.FirstOrDefault(x => x.IdRespuesta == Id && x.EstatusRespuesta != 0);
        }

        public IEnumerable<ForoPreguntasRespuesta> GetForoRespuestaPorIdPregunta(int id)
        {
            return _context.ForoPreguntasRespuestas.Where(x => x.PreguntaRespuesta == id && x.EstatusRespuesta != 0).OrderByDescending(x => x.FechaRespuesta);
        }

        public bool GuardarCambio(string usuario)
        {
            if(_context.SaveChanges() >= 0)
            {
                _accion.Usuario_Modifico_BD(usuario);
                return true;
            }
                
            return false; 
        }
    }
}