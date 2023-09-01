using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BlogApi.Interfaces;
using AutoMapper;
using BlogApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using BlogApi.Security;

namespace BlogApi.Controllers
{
    [Route("api/{Controller}")]
    [ApiController]
    [Authorize]
    public class IndexController : ControllerBase
    {
        private readonly IPublicacionesRepo _publicacion;
        private readonly IDocumentosRepo _documento;
        private readonly IMapper _mapper;
        private readonly IForoPreguntasRepo _pregunta;
        private readonly IUsuariosRep _usuario;

        public IndexController(IPublicacionesRepo publicacion,
                               IDocumentosRepo documento,
                               IMapper mapper,
                               IForoPreguntasRepo pregunta,
                               IUsuariosRep usuario)
        {
            _documento = documento;
            _mapper = mapper;
            _publicacion = publicacion;
            _pregunta = pregunta;
            _usuario = usuario;
        }

        [HttpGet("Publicaciones/Tipo/{tipo}")]
        public ActionResult<List<PublicacionLeeDto>> GetPublicacionesPorTipo(int tipo)
        {
            if(tipo == 0)
                return BadRequest();
                
            return Ok(_publicacion.GetPublicacionPorTipo(tipo));
        }

        [HttpGet("Publicaciones/{id}")]
        public ActionResult<PublicacionLeeDto> GetPublicacionesPorId(int id)
        {
            if(id == 0)
                return BadRequest();
                
            var publicacionModel = _publicacion.GetPublicacionPorId(id);

            if(publicacionModel == null)
                return NotFound();

            var publicacion = _mapper.Map<PublicacionLeeDto>(publicacionModel);

            var usuario = _usuario.GetUsuarioPorId(publicacion.UsuarioPubli);

            var nombrecompletoDec = RSAValidator.Decryption(usuario.NombresUsua) + " " + RSAValidator.Decryption(usuario.ApellidosUsua);
            publicacion.NombreCompleto = nombrecompletoDec;
            publicacion.ImagenNombrePubli = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Images/" + publicacion.ImagenNombrePubli;

            return Ok(publicacion);
        }

        [HttpGet("Publicaciones/Documentos/")]
        public ActionResult<List<DocumentoLeeDto>> GetDocumentos()
        {
            var documentosModel = _documento.GetDocumento();

            if(documentosModel == null)
                return NotFound();

            var documentos = _mapper.Map<List<DocumentoLeeDto>>(documentosModel);

            for(int i = 0; i < documentos.Count; i++)
            {
                documentos[i].FileDoc = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Docs/" + documentos[i].FileDoc;
            }

            return Ok(documentos);
        }

        [HttpGet("Publicaciones/Documentos/{id}")]
        public ActionResult<DocumentoLeeDto> GetDocumentosId(int id)
        {
            if(id == 0)
                return BadRequest();
                
            var documentoModel = _documento.GetDocumentoPorId(id);

            if(documentoModel == null)
                return NotFound();
            
            var documento = _mapper.Map<DocumentoLeeDto>(documentoModel);

            documento.FileDoc = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Docs/" + documento.FileDoc;

            return Ok(documento);
        }

        [HttpGet("Tags")]
        public ActionResult<string[]> GetTags()
        {
            return Ok(_pregunta.GetForoPreguntaTags());
        }
    }
}