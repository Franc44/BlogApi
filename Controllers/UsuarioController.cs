using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BlogApi.Models;
using BlogApi.Interfaces;
using AutoMapper;
using BlogApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.JsonPatch;
using BlogApi.Tools;
using static System.Net.Mime.MediaTypeNames;
using BlogApi.Security;

namespace BlogApi.Controllers
{
    [Route("api/Usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase 
    {
        private readonly IUsuariosRep _usuarioRepo;
        private readonly IMapper _mapper;
        private readonly BlogHCContext _context;
        public UsuarioController(IUsuariosRep usuarioRepo, IMapper mapper, BlogHCContext context)
        {
            _context = context;
            _usuarioRepo = usuarioRepo;
            _mapper = mapper;
        }

        [Authorize(Roles = "Master,Super,Editor")]
        [HttpGet]
        public ActionResult GetTodosUsuarios()
        {
            var UsuarioList = _usuarioRepo.GetUsuarios().ToList();
            if(!User.IsInRole("Master"))
            {
                var listusuario = _mapper.Map<List<UsuarioLeeDto>>(UsuarioList);
                for(int i = 0; i < UsuarioList.Count(); i++)
                {
                    listusuario[i].Rol = UsuarioList[i].TipoUsua == 0 ? "Master" : UsuarioList[i].TipoUsua == 1 ? "Super" : UsuarioList[i].TipoUsua == 2 ? "Editor" : "Comun";
                }

                return Ok(listusuario);        
            }
            
            return Ok(UsuarioList);
        }

        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpGet("{id}", Name = "GetUsuario")]
        public ActionResult<UsuarioCreadto> GetUsuario(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest();

            if((User.IsInRole("Comun") || User.IsInRole("Editor"))&& id != User.Identity.Name)
                return StatusCode(403,$"¿Otra vez tú?, ya te dije que no puedes.");
                
            var Usuario = _usuarioRepo.GetUsuarioPorId(id);

            if(User.IsInRole("Super") && Usuario.TipoUsua == 0)
                return StatusCode(403, $"¿Qué andas viendo aquí carnal?");

            if(Usuario != null)
            {
                var usuarioparse = _mapper.Map<UsuarioCreadto>(Usuario);
                usuarioparse.ContraUsua = new byte[0];
                usuarioparse.ProfilePicture = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Profiles/" + usuarioparse.ProfilePicture;
                return Ok(usuarioparse);
            }
            return NotFound();
        }

        [Authorize(Roles = "Master,Super,Libre")]
        [HttpPost]
        public ActionResult AgregaUsuario([FromForm]UsuarioCreadto usuarioCreadto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }  

            //*Verificación de los permisos para crear los usuarios en el sistema.
            string roles = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            if(roles == "Libre" && usuarioCreadto.TipoUsua <= 2)
                return StatusCode(403,$"¿Qué haces campeón?, Esto no es para ti.");

            if(roles == "Super" && usuarioCreadto.TipoUsua == 0)
                return StatusCode(403,$"¿Qué haces campeón?, Esto nos es para ti.");    
            //*Fin Verificación

            //Si se desea una foto de perfil personalizada, se deja vacio el campo "ProfilePicture", además del campo "File"
            if(string.IsNullOrEmpty(usuarioCreadto.ProfilePicture))
            {
                //Aquí se llama al método que lo guarda dentro del servidor
                string fileName = FilesManager.GuardaArchivo(usuarioCreadto.File, "Profiles"); 
                //Se verifica que el servidor lo haya podido guardar
                if(string.IsNullOrEmpty(fileName))
                    return StatusCode(500, $"Oh no! :( \nRevisa si enviaste el archivo, porfa.");

                //Se modifica el campo "ProfilePicture" por el nombre con el que fue guardado en el servidor
                usuarioCreadto.ProfilePicture = fileName;
            }
            
            //!Aquí se convierte el objeto entrante de la consulta a el objeto generado por la base de datos
            var usuarioModel = _mapper.Map<Usuario>(usuarioCreadto);

            if(!_usuarioRepo.CrearUsuario(usuarioModel))
                return StatusCode(302, $"Este usuario ya ha sido registrado.");

            if(!_usuarioRepo.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");
            
            return NoContent();
        }

        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPut]
        [Route("{id}")]
        public ActionResult ActualizaUsuario(string id, [FromForm]UsuarioCreadto usuarioCreadto)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest();

            //Verifica que el usuario común no cometa pendejadas(si es que es desarrollador)
            if((User.IsInRole("Comun") || User.IsInRole("Editor")) && id != User.Identity.Name)
                return StatusCode(403, $"Nop, no puedes hacer esto.");

            if(User.IsInRole("Super") && usuarioCreadto.TipoUsua == 0)
                return StatusCode(403, $"jajjaja crees que por ser super vas a poder moverme, tas pendejo.");    

            var existeUsuario = _usuarioRepo.GetUsuarioPorId(id);

            if(existeUsuario == null)
                return NotFound();

            //Contol de lo que puede modificar cada usuario.
            if(!User.IsInRole("Master"))
            {
                existeUsuario.NombresUsua = usuarioCreadto.NombresUsua;
                existeUsuario.ApellidosUsua = usuarioCreadto.ApellidosUsua;
                existeUsuario.EmailUsua = usuarioCreadto.EmailUsua;
            }

            //Si se desea una foto de perfil personalizada, se deja vacio el campo "ProfilePicture", además del campo "File"
            if(usuarioCreadto.File != null)
            {
                //Se elimina el archivo previo 
                //Este metodo estatico requiere el nombre de la imagen guardada en la base de datos y el nombre del directorio donde se guarda
                FilesManager.EliminaArchivo(usuarioCreadto.ProfilePicture, "Profiles");
                //Aquí se llama al método que lo guarda dentro del servidor
                string fileName = FilesManager.GuardaArchivo(usuarioCreadto.File, "Profiles"); 
                //Se verifica que el servidor lo haya podido guardar
                if(string.IsNullOrEmpty(fileName))
                    return StatusCode(500, $"Oh no! :( \nRevisa si enviaste el archivo, porfa.");

                //Se modifica el campo "ProfilePicture" por el nombre con el que fue guardado en el servidor
                usuarioCreadto.ProfilePicture = fileName;
            }

            if(User.IsInRole("Master"))
                _mapper.Map(usuarioCreadto, existeUsuario);

            _usuarioRepo.ActualizaUsuario(existeUsuario);

            if(!_usuarioRepo.GuardarCambio(User.Identity.Name))
                return StatusCode(500, $"Oh no! :(");

            return NoContent();
        }

        [Authorize(Roles = "Master,Super")]
        [HttpPatch]
        [Route("{id}")]
        public ActionResult ActualizacionParcialUsuario(string id, JsonPatchDocument<UsuarioCreadto> patchDocument)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest();

            var existeUsuario = _usuarioRepo.GetUsuarioPorId(id);
            if(existeUsuario == null)
            {
                return NotFound();
            }
            
            var campoaModificar = patchDocument.Operations.FirstOrDefault();

            //if(User.IsInRole("Editor") && id != User.Identity.Name && existeUsuario.TipoUsua < 3 && campoaModificar.path != "/estatusUsua")
              //  return StatusCode(403, $"Nop, no puedes hacer esto.");

            var usuarioToPatch = _mapper.Map<UsuarioCreadto>(existeUsuario);
            patchDocument.ApplyTo(usuarioToPatch, ModelState);

            if(!TryValidateModel(patchDocument))
                return ValidationProblem(ModelState);

            _mapper.Map(usuarioToPatch, existeUsuario);    

            _usuarioRepo.ActualizaUsuario(existeUsuario);
            _usuarioRepo.GuardarCambio(User.Identity.Name);

            return NoContent();
        }

        [Authorize(Roles = "Master,Super")]
        [HttpDelete("{id}")]
        public ActionResult EliminaUsuario(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest();

            var UsuarioAEliminar = _usuarioRepo.GetUsuarioPorId(id);

            if(UsuarioAEliminar == null)
            {
                return NotFound();
            }

            _usuarioRepo.EliminaUsuario(UsuarioAEliminar);
            _usuarioRepo.GuardarCambio(User.Identity.Name);

            return NoContent();
        }

        //Recuperacion de contraseñas
        [AllowAnonymous]
        [HttpGet("Recuperacion/Contrasena/{id}")]
        public ActionResult RecuperarContra(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest();

            string usuario = "";

            if(_usuarioRepo.RecuperarContra(id, out usuario))
                return Ok(usuario);
            else
                return NotFound();      
        }

        [AllowAnonymous]
        [HttpGet("Recuperacion/Contrasena/Clave/")]
        public ActionResult ValidacionClaveRecuperacion(string clave, string usuario)
        {
            if(string.IsNullOrEmpty(clave) || string.IsNullOrEmpty(usuario))
                return BadRequest();

            string[] tokenUsuario = _usuarioRepo.ValidacionClaveRecupera(clave, usuario);
            if(tokenUsuario != null)
                return Ok(tokenUsuario);

            return NotFound();    
        }

        [Authorize(Roles = "Master,Super,Editor,Comun")]
        [HttpPatch]
        [Route("Recuperacion/Contrasena/Usuario/{id}")]
        public ActionResult ActualizaContrasena(string id, JsonPatchDocument<UsuarioCreadto> patchDocument)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest();
                
            //Verifica que el usuario común no cometa pendejadas(si es que es desarrollador)
            if((User.IsInRole("Comun") || User.IsInRole("Editor")) && id != User.Identity.Name)
                return StatusCode(403, $"Nop, no puedes hacer esto.");

            if(User.IsInRole("Super") && User.Identity.Name == "Master")
                return StatusCode(403, $"jajjaja crees que por ser super vas a poder moverme, tas pendejo.");    

            var existeUsuario = _usuarioRepo.GetUsuarioPorId(id);
            if(existeUsuario == null)
            {
                return NotFound();
            }
            
            var campoaModificar = patchDocument.Operations.FirstOrDefault();
            if(campoaModificar.path != "/contraUsua")
                return StatusCode(403, $"¿Qué haces campeón?");

            var usuarioToPatch = _mapper.Map<UsuarioCreadto>(existeUsuario);
            patchDocument.ApplyTo(usuarioToPatch, ModelState);

            if(!TryValidateModel(patchDocument))
                return ValidationProblem(ModelState);

            _mapper.Map(usuarioToPatch, existeUsuario);    

            _usuarioRepo.ActualizaUsuario(existeUsuario);
            _usuarioRepo.GuardarCambio(User.Identity.Name);

            return NoContent();
        }
    }
}