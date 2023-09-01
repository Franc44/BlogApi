using BlogApi.Tools;
using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;
using BlogApi.Interfaces;
using AutoMapper;
using BlogApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Cors;

namespace BlogApi.Controllers
{
    [Controller]
    [Route("api/{controller}")]
    public class ForoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IForoPreguntasRepo _foro;
        private readonly IForoPreguntasRespuestasRepo _foroR;
        private readonly ILikesPR _likes;
        private readonly IUsuariosRep _user;
        public ForoController(IMapper mapper, IForoPreguntasRepo foro, IForoPreguntasRespuestasRepo foroR, ILikesPR likes, IUsuariosRep user)
        {
            _mapper = mapper;
            _foro = foro;
            _foroR = foroR;
            _likes = likes;
            _user = user;
        }

        //Gets 
        //Generales
        //Recuperar todas las preguntas que se han hecho
        [Authorize]
        [HttpGet("Pregunta/Todo")]
        public ActionResult GetTodo()
        {
            var preguntasTem = _foro.GetForoPregunta().ToList();
            var preguntas = _mapper.Map<List<ForoPreguntaLeeDto>>(preguntasTem);

            for(int i = 0; i < preguntas.Count; i++)
            {
                //Se agrega los tags en forma de arreglo al objeto
                preguntas[i].Tags = preguntasTem[i].EtiquetasPregunta.Split('.');

                //Solo se agrega en el objeto saliente cuantas respuestas tiene
                preguntas[i].ContadorRespuestas = _foroR.GetForoRespuestaPorIdPregunta(preguntas[i].IdPregunta).Count();

                //Se envia la ruta de la imagen del usuario
                string usuarioPregunton = _user.GetUsuarioPorId(preguntas[i].UsuarioPregunta).ProfilePicture; 
                preguntas[i].ProfilePictureuUsuario = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Profiles/" + usuarioPregunton;

                //Por último se checa si el usuario que esta consultando las preguntas tenga like en alguna
                var usuarioLog = User.Identity.Name;
                if(User.IsInRole("Invitado"))
                    preguntas[i].LikeDelUsuarioLog = false;
                else
                {
                    preguntas[i].LikeDelUsuarioLog = _likes.ExisteLikePregunta(User.Identity.Name, preguntas[i].IdPregunta) == null ? false : true; 
                }
            }

            return Ok(preguntas);
        }

        [Authorize]
        [HttpGet("Pregunta/Tag/{tag}")]
        public ActionResult GetTodoTag(string tag)
        {
            if(string.IsNullOrEmpty(tag))
                return BadRequest();

            var preguntasTem = _foro.GetForoPreguntaPorTag(tag).ToList();
            var preguntas = _mapper.Map<List<ForoPreguntaLeeDto>>(preguntasTem);

            for(int i = 0; i < preguntas.Count; i++)
            {
                //Se agrega los tags en forma de arreglo al objeto
                preguntas[i].Tags = preguntasTem[i].EtiquetasPregunta.Split('.');

                //Solo se agrega en el objeto saliente cuantas respuestas tiene
                preguntas[i].ContadorRespuestas = _foroR.GetForoRespuestaPorIdPregunta(preguntas[i].IdPregunta).Count();

                //Se envia la ruta de la imagen del usuario
                string usuarioPregunton = _user.GetUsuarioPorId(preguntas[i].UsuarioPregunta).ProfilePicture; 
                preguntas[i].ProfilePictureuUsuario = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Profiles/" + usuarioPregunton;


                //Por último se checa si el usuario que esta consultando las preguntas tenga like en alguna
                var usuarioLog = User.Identity.Name;
                if(User.IsInRole("Invitado"))
                    preguntas[i].LikeDelUsuarioLog = false;
                else
                {
                   preguntas[i].LikeDelUsuarioLog = _likes.ExisteLikePregunta(User.Identity.Name, preguntas[i].IdPregunta) == null ? false : true; 
                }
            }

            return Ok(preguntas);
        }
        
        [Authorize]
        [HttpGet("Pregunta/{id}")]
        public ActionResult GetPreguntaId(int id)
        {
            if(id == 0)
                return BadRequest();

            var preguntaTem = _foro.GetForoPreguntaPorId(id);
            var pregunta = _mapper.Map<ForoPreguntaLeeDto>(preguntaTem);

            //Se agrega los tags en forma de arreglo al objeto
            pregunta.Tags = preguntaTem.EtiquetasPregunta.Split('.');

            //Se envia la ruta de la imagen del usuario
            string usuarioPregunton = _user.GetUsuarioPorId(pregunta.UsuarioPregunta).ProfilePicture; 
            pregunta.ProfilePictureuUsuario = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Profiles/" + usuarioPregunton;
            
            //Se traen las respuestas de la base
            var respuestas = _foroR.GetForoRespuestaPorIdPregunta(pregunta.IdPregunta).OrderBy(x => x.FechaRespuesta).ToList();
            //Se mapean en el objeto resultante 
            var respuestasMapper = _mapper.Map<List<ForoPreguntasRespuestaDTO>>(respuestas);
            //Se cuentan las respuestas
            pregunta.ContadorRespuestas = respuestasMapper.Count();

            //Se modifica el campo LikesUsuarioLog para saber si se ha dado like a la respuesta
            for(int i = 0; i < respuestasMapper.Count; i++)
            {
                respuestasMapper[i].LikeDelUsuarioLog = _likes.ExisteLikeRespuesta(User.Identity.Name, respuestasMapper[i].IdRespuesta) == null ? false : true;

                //Agrega la ruta de la imagen del usuario que pregunta
                string usuarioPreguntonRes = _user.GetUsuarioPorId(respuestasMapper[i].UsuarioRespuesta).ProfilePicture; 
                respuestasMapper[i].ProfilePicture = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Profiles/" + usuarioPreguntonRes;
            }

            pregunta.ForoPreguntasRespuesta = respuestasMapper;
            pregunta.LikeDelUsuarioLog = _likes.ExisteLikePregunta(User.Identity.Name, pregunta.IdPregunta) == null ? false : true;

            return Ok(pregunta);
        }

       /* [Authorize]
        [HttpGet("Pregunta/Busqueda/{busqueda}")]
        public ActionResult GetTodoBusqueda(string busqueda)
        {
            if(string.IsNullOrEmpty(busqueda))
                return BadRequest();

            var preguntas = _foro.GetForoPreguntasPorBusqueda(busqueda, User.Identity.Name).ToList();

            return Ok(preguntas);
        }*/
        
        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpGet("Pregunta/Usuario/{usuario}")]
        public ActionResult GetPreguntasPosUsuario(string usuario)
        {
            if(string.IsNullOrEmpty(usuario))
                return BadRequest();

            var preguntas = _foro.GetForoPreguntasPorUsuario(usuario).ToList();

            return Ok(preguntas);
        }

        //Post
        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPost("Pregunta/Agrega")]
        public ActionResult AgregaPregunta([FromBody]ForoPreguntaDTO preguntaDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest();   

            var preguntaModel = _mapper.Map<ForoPregunta>(preguntaDTO);
            _foro.CrearForoPregunta(preguntaModel);
            

            if(!_foro.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPost("Respuesta/Agrega")]
        public ActionResult AgregaRespuesta([FromBody]ForoPreguntasRespuestaDTO respuestaDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var respuestaModel = _mapper.Map<ForoPreguntasRespuesta>(respuestaDTO);
            
            _foroR.CrearForoRespuesta(respuestaModel);

            
            if(!_foroR.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            //return NoContent();
            return Ok(respuestaModel);
        }

        //Le pones like a una pregunta o respuesta
        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPost("Pregunta/Like")]
        public ActionResult AgregaLikesPregunta([FromBody]FPLikesDto fPLikesDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var likesModel = _mapper.Map<ForoPreguntasLike>(fPLikesDto);
            
            _likes.CrearLikeFP(likesModel);
            
            if(!_likes.GuardarCambio())
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPost("Respuesta/Like")]
        public ActionResult AgregaLikesRespuesta([FromBody]FPRLikesDto fPRLikesDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var likesModel = _mapper.Map<ForoPreguntasRespuestasLike>(fPRLikesDto);
            
            _likes.CrearLikeFPR(likesModel);
            
            if(!_likes.GuardarCambio())
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        //Put - Patch
        //Actualiza completo
        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPut("Pregunta/Actualiza/{id}")]
        public ActionResult ActualizaPregunta(int id, [FromBody]ForoPreguntaDTO preguntaCreadto)
        {
            if(id == 0) return BadRequest();

            //Verifica que el usuario común no cometa pendejadas(si es que es desarrollador)
            if((User.IsInRole("Comun") || User.IsInRole("Editor")) && User.Identity.Name != preguntaCreadto.UsuarioPregunta)
                return StatusCode(403, $"Nop, no puedes hacer esto.");   

            var existePregunta = _foro.GetForoPreguntaPorId(id);
            if(existePregunta == null)
                return NotFound();

            //Contol de lo que puede modificar cada usuario.
            if(User.IsInRole("Comun") || User.IsInRole("Editor"))
            {
                existePregunta.TituloPregunta = preguntaCreadto.TituloPregunta;
                existePregunta.DescripcionPregunta = preguntaCreadto.DescripcionPregunta;
                existePregunta.EtiquetasPregunta = preguntaCreadto.EtiquetasPregunta;
            }

            if(User.IsInRole("Super") || User.IsInRole("Master"))
                _mapper.Map(preguntaCreadto, existePregunta);

            _foro.ActualizaForoPregunta(existePregunta);
            
            if(!_foro.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPut("Respuesta/Actualiza/{id}")]
        public ActionResult ActualizaRespuesta(int id, [FromBody]ForoPreguntasRespuestaDTO respuestaCreadto)
        {
            if(id == 0)
                return BadRequest();
                
            //Verifica que el usuario común no cometa pendejadas(si es que es desarrollador)
            if((User.IsInRole("Comun") || User.IsInRole("Editor")) && User.Identity.Name != respuestaCreadto.UsuarioRespuesta)
                return StatusCode(403, $"Nop, no puedes hacer esto.");   

            var existeRespuesta = _foroR.GetForoRespuestaPorId(id);
            if(existeRespuesta == null)
                return NotFound();

            //Contol de lo que puede modificar cada usuario.
            if(User.IsInRole("Comun") || User.IsInRole("Editor"))
            {
                existeRespuesta.TextoRespuesta = respuestaCreadto.TextoRespuesta;
            }

            if(User.IsInRole("Super") || User.IsInRole("Master"))
                _mapper.Map(respuestaCreadto, existeRespuesta);

            _foroR.ActualizaForoRespuesta(existeRespuesta);
            
            if(!_foroR.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        //Para Likes y cambios de estatus: 0-Abierta; 1-Cerrada; 2-Eliminada
        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPatch("Pregunta/Parcial/{id}")]
        public ActionResult ActualizacionParcialPregunta(int id, [FromBody]JsonPatchDocument<ForoPreguntaDTO> patchDocument)
        {
            if(id == 0)
                return BadRequest();
                
            var existePregunta = _foro.GetForoPreguntaPorId(id);
            if(existePregunta == null)
            {
                return NotFound();
            }

            var campoaModificar = patchDocument.Operations.FirstOrDefault().path;

            if(User.IsInRole("Comun") && ((existePregunta.UsuarioPregunta != User.Identity.Name && campoaModificar != "/estatusPregunta") || campoaModificar != "/likesPregunta"))
                return StatusCode(403, $"Nop, no puedes hacer esto.");

            var preguntaToPatch = _mapper.Map<ForoPreguntaDTO>(existePregunta);
            patchDocument.ApplyTo(preguntaToPatch, ModelState);

            if(!TryValidateModel(patchDocument))
                return ValidationProblem(ModelState);

            _mapper.Map(preguntaToPatch, existePregunta);    
            _foro.ActualizaForoPregunta(existePregunta);
            
            if(!_foro.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");
         
            return NoContent();
        }

        //Para respuestas: likes, poner correcta la respuesta o eliminar una respuesta cambiando su estatus
        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPatch("Respuesta/Parcial/{id}")]
        public ActionResult ActualizacionParcialRespuesta(int id, JsonPatchDocument<ForoPreguntasRespuestaDTO> patchDocument)
        {
            if(id == 0)
                return BadRequest();
                
            var existeRespuesta = _foroR.GetForoRespuestaPorId(id);
            if(existeRespuesta == null)
            {
                return NotFound();
            }

            var campoaModificar = patchDocument.Operations.FirstOrDefault().path;

            if(User.IsInRole("Comun") && ((existeRespuesta.PreguntaRespuestaNavigation.UsuarioPregunta != User.Identity.Name && campoaModificar != "/correctaRespuesta") 
            || campoaModificar != "/likesRespuesta" || (existeRespuesta.UsuarioRespuesta != User.Identity.Name && campoaModificar != "/estatusRespuesta")))
                return StatusCode(403, $"Nop, no puedes hacer esto.");

            var respuestaToPatch = _mapper.Map<ForoPreguntasRespuestaDTO>(existeRespuesta);
            patchDocument.ApplyTo(respuestaToPatch, ModelState);

            if(!TryValidateModel(patchDocument))
                return ValidationProblem(ModelState);

            _mapper.Map(respuestaToPatch, existeRespuesta);    
            _foroR.ActualizaForoRespuesta(existeRespuesta);
            
            if(!_foroR.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");
         
            return NoContent();
        }

        //Delete
        //Solo serán efectuados por usuarios con altos privilegios
        [Authorize(Roles = "Master,Super,Editor")]
        [HttpDelete("Pregunta/Elimina/{id}")]
        public ActionResult EliminaPreguntaRespuestas(int id)
        {
            if(id == 0)
                return BadRequest();
                
            var preguntaAEliminar = _foro.GetForoPreguntaPorId(id);
            if(preguntaAEliminar == null)
                return NotFound();

            _foro.EliminaForoPregunta(preguntaAEliminar);
            _foro.GuardarCambio(User.Identity.Name);

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor")]
        [HttpDelete("Respuesta/Elimina/{id}")]
        public ActionResult EliminaRespuestas(int id)
        {
            if(id == 0)
                return BadRequest();
                
            var respuestaAEliminar = _foroR.GetForoRespuestaPorId(id);
            if(respuestaAEliminar == null)
                return NotFound();

            _foroR.EliminaForoRespuesta(respuestaAEliminar);
            _foroR.GuardarCambio(User.Identity.Name);

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpDelete("Pregunta/Likes/Elimina/{id}")]
        public ActionResult EliminaPreguntaLikes(int id)
        {
            if(id == 0)
                return BadRequest();

            var preguntaLikeAEliminar = _likes.ExisteLikePregunta(User.Identity.Name, id);
            if(preguntaLikeAEliminar == null)
                return NotFound();

            _likes.EliminaLikeFP(preguntaLikeAEliminar);
            if(!_likes.GuardarCambio())
                return StatusCode(500, "Oh no! :(");

            return NoContent();
        }
        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpDelete("Respuesta/Likes/Elimina/{id}")]
        public ActionResult EliminaRespuestasLikes(int id)
        {
            if(id == 0)
                return BadRequest();

            var respuestaLikeAEliminar = _likes.ExisteLikeRespuesta(User.Identity.Name, id);
            if(respuestaLikeAEliminar == null)
                return NotFound();

            _likes.EliminaLikeFPR(respuestaLikeAEliminar);
            if(!_likes.GuardarCambio())
                return StatusCode(500, "Oh no! :(");

            return NoContent();
        }
    }
}