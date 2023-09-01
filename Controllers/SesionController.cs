using System;
using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;
using BlogApi.Interfaces;
using BlogApi.DTOs;
using BlogApi.Security;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Linq;

namespace BlogApi.Controllers
{
    [Route("api/Sesion")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private readonly ISesionRep _sesionRep;
        public SesionController(ISesionRep sesionRep)
        {
            _sesionRep = sesionRep;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Autenticacion")]
        public ActionResult<UsuarioLeeDto> Autenticacion([FromBody] InicioSesion inicioSesion)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            //*Se decifran los datos de entrada
            string username = RSAValidator.Decryption(inicioSesion.Usuario);
            string passw = RSAValidator.Decryption(inicioSesion.Contra);
            //Se comprueba que los arreglos de byte sean correctos, haciendo una comprobación de que las 
            //variables de arriba no esten vacias o nulas.
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passw))
                return StatusCode(500, "What are u doing man?");

            //Se manda a llamar el metodo que comprueba que el usuario este en la base de datos
            var inicioUsuario = _sesionRep.Autenticacion(username, passw);
            if(inicioUsuario == null)
                return NotFound();

             Response.Cookies.Append(
                "Token",
                inicioUsuario.Token,
                new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Path = "/",
                    Secure = true,
                    Expires = inicioUsuario.ExpiresDate
                }
            );

            return Ok(inicioUsuario);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public ActionResult<UsuarioLeeDto> Refresh([FromBody] InicioSesion inicioSesion)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            try
            {
                //Primero se descifra el byte[] entrante para comprobar que exista el usuario
                string username = RSAValidator.Decryption(inicioSesion.Usuario);
                //Ademas se comprueba que sea válido el arreglo entrante porque el metodo de arriba 
                //devuelve nulo si es que tiene una mala estructura 
                if(username == null)
                    return StatusCode(500, "What are u doing man?");

                var refresh = _sesionRep.RefreshToken(username, inicioSesion.Token);
                if(refresh == null)
                    return NotFound();

                return Ok(refresh);  
            }
            catch
            {
                return StatusCode(500, "What are u doing man?");
            }  
        }

    }
}