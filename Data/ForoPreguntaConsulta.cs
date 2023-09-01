using System.Linq;
using System.Collections.Generic;
using System;
using BlogApi.Interfaces;
using BlogApi.Models;
using AutoMapper;
using BlogApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Data
{
    public class ForoPreguntaConsulta : IForoPreguntasRepo
    {
        private readonly BlogHCContext _context;
        private readonly IAccionUsuario _accion;
        private readonly IMapper _mapper; 
        private readonly ILikesPR _likes;

        public ForoPreguntaConsulta(BlogHCContext context, IAccionUsuario accion, IMapper mapper, ILikesPR likes)
        {
            _context = context;
            _accion = accion;
            _mapper = mapper;
            _likes = likes;
        }

        public void ActualizaForoPregunta(ForoPregunta pregunta)
        {
            
        }

        public void CrearForoPregunta(ForoPregunta pregunta)
        {
            if(pregunta == null)
                throw new ArgumentNullException(nameof(pregunta));

            _context.ForoPreguntas.Add(pregunta);
        }

        public void EliminaForoPregunta(ForoPregunta pregunta)
        {
            if(pregunta == null)
                throw new ArgumentNullException(nameof(pregunta));

            var respuestas = _context.ForoPreguntasRespuestas.Where(x => x.PreguntaRespuesta == pregunta.IdPregunta);
            var likesPregunta = _context.ForoPreguntasLikes.Where(x => x.IdPreguntaFpl == pregunta.IdPregunta);

            List<ForoPreguntasRespuestasLike> likesRespuestaPregunta = new List<ForoPreguntasRespuestasLike>();
            foreach(var item in respuestas)
            {
                var likesRespuestas = _context.ForoPreguntasRespuestasLikes.Where(x => x.IdRespuestaFprl == item.IdRespuesta);
                likesRespuestaPregunta.AddRange(likesRespuestas);
            }

            _context.RemoveRange(likesRespuestaPregunta);
            _context.RemoveRange(respuestas);
            _context.RemoveRange(likesPregunta);
            _context.Remove(pregunta);
        }

        public IEnumerable<ForoPregunta> GetForoPregunta()
        {
            return _context.ForoPreguntas.Where(x => x.EstatusPregunta != 2).OrderByDescending(x => x.FechaPregunta);
        }

        public ForoPregunta GetForoPreguntaPorId(int Id)
        {
            return _context.ForoPreguntas.FirstOrDefault(x => x.IdPregunta == Id && x.EstatusPregunta != 2);
        }

        public IEnumerable<ForoPregunta> GetForoPreguntaPorTag(string tag)
        {
            //Traes la lista completa de las preguntas desde la base de datos
            var preguntas = _context.ForoPreguntas.Where(x => x.EstatusPregunta != 2).OrderByDescending(x =>x.FechaPregunta).ToList();
            
            //Recorres una lista identitica a la consultada a la de arriba para obtener el item a quitar
            //No se puede remover un item y recorrer la lista
            foreach(var item in _context.ForoPreguntas.OrderByDescending(x =>x.FechaPregunta).ToList())
            {
                //Se separa la cadena que se trae del campo Etiquetas del item en el que va
                string[] tagsPregunta = item.EtiquetasPregunta.Split('.');
                
                //Se utiliza esta variable para saber si es que encontro la etiqueta en este item
                bool existe = false;
                //Una vez separado e insertado en un arreglo tipo string, se recorre
                for(int i = 0; i < tagsPregunta.Length; i++)
                {
                    //se recorre el arrglo en busca del tag solicitado
                    if(tagsPregunta[i] == tag)
                        existe = true;
                }
                //Si no encontro ninguna etiqueta igual a la requerida en el item, se quita de la lista 
                if(!existe) 
                    preguntas.Remove(item);
            }

            return preguntas;
        }

        public IEnumerable<ForoPreguntaLeeDto> GetForoPreguntasPorBusqueda(string buscaString, string usuario)
        {
            var preguntasModel = _context.ForoPreguntas.FromSqlRaw("Execute Busqueda_FT @Busqueda = '" + buscaString + "'").Where(x => x.EstatusPregunta != 2);
            //var preguntasModelList = preguntasModel.ToList();
            var preguntas = _mapper.Map<IEnumerable<ForoPreguntaLeeDto>>(preguntasModel);
            
            /*for(int i = 0; i < preguntas.Count(); i++)
            {
                //Se agrega los tags en forma de arreglo al objeto
                preguntas..Tags = preguntasModelList[i].EtiquetasPregunta.Split('.');

                //Se asignan el número de respuestas que poseé una pregunta
                preguntas[i].ContadorRespuestas = _context.ForoPreguntasRespuestas.Where(x => x.PreguntaRespuesta == preguntas[i].IdPregunta && x.EstatusRespuesta != 0).Count();

                //Por último se checa si el usuario que esta consultando las preguntas tenga like en alguna
                if(usuario == "Invitado")
                    preguntas[i].LikeDelUsuarioLog = false;
                else
                {
                   preguntas[i].LikeDelUsuarioLog = _likes.ExisteLikePregunta(usuario, preguntas[i].IdPregunta) == null ? false : true; 
                }
            }*/       

            return preguntas;
        }

        public IEnumerable<ForoPreguntaLeeDto> GetForoPreguntasPorUsuario(string usuario)
        {
            var preguntasModel = _context.ForoPreguntas.Where(x => x.UsuarioPregunta == usuario && x.EstatusPregunta != 2).ToList();

            var preguntas = _mapper.Map<List<ForoPreguntaLeeDto>>(preguntasModel);
            
            for(int i = 0; i < preguntas.Count; i++)
            {
                //Se agrega los tags en forma de arreglo al objeto
                preguntas[i].Tags = preguntasModel[i].EtiquetasPregunta.Split('.');

                //Se asignan el número de respuestas que poseé una pregunta
                preguntas[i].ContadorRespuestas = _context.ForoPreguntasRespuestas.Where(x => x.PreguntaRespuesta == preguntas[i].IdPregunta && x.EstatusRespuesta != 0).Count();

                //Se envia la ruta de la imagen del usuario
                string usuarioPregunton = _context.Usuarios.Where(x => x.IdUsuario == preguntas[i].UsuarioPregunta).FirstOrDefault().ProfilePicture; 
                preguntas[i].ProfilePictureuUsuario = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Profiles/" + usuarioPregunton;
                
                 //Por último se checa si el usuario que esta consultando las preguntas tenga like en alguna
                if(usuario == "Invitado")
                    preguntas[i].LikeDelUsuarioLog = false;
                else
                {
                   preguntas[i].LikeDelUsuarioLog = _likes.ExisteLikePregunta(usuario, preguntas[i].IdPregunta) == null ? false : true; 
                }
            }       

            return preguntas;
        }

        public string[] GetForoPreguntaTags()
        {
            //Se inicializa el arreglo de retorno con las etiquetas con cualquier inicialización de arreglo
            //así no provoca el error en el return
            string[] tagsPregunta = new string[0];

            //Se recorre todos los registros de la tabla de la tabla de preguntas
            foreach (var item in _context.ForoPreguntas.ToList())
            {
                //En cada item, se desconcatena el campo de etiqueta
                string[] tagDentroItem = item.EtiquetasPregunta.Split('.');
                //Posteriormente, el arrehlo de arriba se une al arreglo de retorno, usando la función de Linq, Union
                //Una gran ventaja de utilizar Union, es que excluye los elementos repetidos del arreglo
                //así ya no es necesesario hacer un procedimiento de removimiento de duplicados
                tagsPregunta = tagsPregunta.Union(tagDentroItem).ToArray();
            }

            return tagsPregunta;
        }

        public bool GuardarCambio(string usuario)
        {
            //Se confirma que haya modificado la base de datos
            if(_context.SaveChanges() >= 0)
            {
                //Posteriormente se modifica el último registro de la tabla de Acciones_Usuarios 
                //así sabemos que usuario fue el que realizo la accion
                _accion.Usuario_Modifico_BD(usuario);
                return true;
            }
                
            return false; 
        }
    }
}