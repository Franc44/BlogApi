using BlogApi.Tools;
using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;
using BlogApi.Interfaces;
using AutoMapper;
using BlogApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BlogApi.Controllers
{
    [Controller]
    [Route("api/{Controller}")]
    public class BlogAdminController : ControllerBase
    {
        private readonly IPublicacionesRepo _publicacion;
        private readonly IDocumentosRepo _documento;
        private readonly IMapper _mapper;
        private readonly IForoPreguntasRespuestasRepo _foroR;
        public BlogAdminController(IPublicacionesRepo publicacion, IMapper mapper, IDocumentosRepo documento, IForoPreguntasRespuestasRepo foroR)
        {
            _publicacion = publicacion;
            _mapper = mapper;
            _documento = documento;
            _foroR = foroR;
        }

        [Authorize(Roles = "Master,Super,Editor")]
        [HttpPost("Publicacion/Agrega")]
        public ActionResult AgregaPublicacion([FromForm]PublicacionDTO publicacion)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            string fileName = "";

            if(string.IsNullOrEmpty(publicacion.ImagenNombrePubli))
            {
                fileName = FilesManager.GuardaArchivo(publicacion.File, "Images");
                    
                if(string.IsNullOrEmpty(fileName))
                    return StatusCode(500, $"Oh no! :( \nRevisa si enviaste el archivo, porfa.");

                publicacion.ImagenNombrePubli = fileName;    
            }
                
            var publicacionModel = _mapper.Map<Publicacione>(publicacion);

            _publicacion.CrearPublicacion(publicacionModel);

            if(!_publicacion.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor")]
        [HttpPost("Documento/Agrega")]
        public ActionResult AgregaDocumento([FromForm]DocumentoDto documento)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            string fileName = FilesManager.GuardaArchivo(documento.File, "Docs");

            if(string.IsNullOrEmpty(fileName))
                return StatusCode(500, $"Oh no! :( \nRevisa si enviaste el archivo, porfa.");

            documento.FileDoc = fileName;

            var documentoModel = _mapper.Map<Documento>(documento);

            _documento.CrearDocumento(documentoModel);

            if(!_documento.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor")]
        [HttpPut("Publicacion/Actualiza/{id}")]
        public ActionResult ActualizaPublicacion(int id, [FromForm]PublicacionDTO publicacion)
        {
            if(id == 0)
                return BadRequest();

            var existePublicacion = _publicacion.GetPublicacionPorId(id);
            if(existePublicacion == null)
                return NotFound();

            if(publicacion.File != null || publicacion.File.Length  > 0)
            {
                FilesManager.EliminaArchivo(existePublicacion.ImagenNombrePubli, "Images");
                string fileName = FilesManager.GuardaArchivo(publicacion.File, "Images");

                if(string.IsNullOrEmpty(fileName))
                    return StatusCode(500, $"Oh no! :( \nRevisa si enviaste el archivo, porfa.");

                publicacion.ImagenNombrePubli = fileName;
            }

            _mapper.Map(publicacion, existePublicacion);

            _publicacion.ActualizaPublicacion(existePublicacion);
            if(!_publicacion.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor")]
        [HttpPut("Documento/Actualiza/{id}")]
        public ActionResult ActualizaDocumento(int id, [FromForm]DocumentoDto documento)
        {
            var existeDocumento = _documento.GetDocumentoPorId(id);
            if(existeDocumento == null)
                return NotFound();

            if(documento.File != null || documento.File.Length > 0)
            {
                bool documentoEliminado = FilesManager.EliminaArchivo(existeDocumento.FileDoc, "Docs");
                string fileName = FilesManager.GuardaArchivo(documento.File, "Docs");

                if(string.IsNullOrEmpty(fileName))
                    return StatusCode(500, $"Oh no! :( \nRevisa si enviaste el archivo, porfa.");

                documento.FileDoc = fileName;
            }

            _mapper.Map(documento,existeDocumento);

            _documento.ActualizaDocumento(existeDocumento);
            if(!_documento.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor")]
        [HttpDelete("Publicacion/Elimina/{id}")]
        public ActionResult EliminaPublicacion(int id)
        {
            if(id == 0)
                return BadRequest();
                
            var publicacionAEliminar = _publicacion.GetPublicacionPorId(id);

            if(publicacionAEliminar == null)
            {
                return NotFound();
            }

            bool publicacionImagenEliminada = FilesManager.EliminaArchivo(publicacionAEliminar.ImagenNombrePubli, "Images");

            if(publicacionImagenEliminada)
            {
                _publicacion.EliminaPublicacion(publicacionAEliminar);
                _publicacion.GuardarCambio(User.Identity.Name);

                return NoContent();
            }
            else
            {
                return StatusCode(500, $"Oh no! :(");
            }
        }

        [Authorize(Roles = "Master,Super,Editor")]
        [HttpDelete("Documento/Elimina/{id}")]
        public ActionResult EliminaDocumento(int id)
        {
            if(id == 0)
                return BadRequest();
                
            var documentoAEliminar = _documento.GetDocumentoPorId(id);

            if(documentoAEliminar == null)
            {
                return NotFound();
            }

            bool documentoImagenEliminado = FilesManager.EliminaArchivo(documentoAEliminar.FileDoc, "Docs");

            if(documentoImagenEliminado)
            {
                _documento.EliminaDocumento(documentoAEliminar);
                _documento.GuardarCambio(User.Identity.Name);
                
                return NoContent();
            }
            else
                return StatusCode(500, $"Oh no! :(");

        }
    }
}