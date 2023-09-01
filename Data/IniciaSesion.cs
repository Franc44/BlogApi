using BlogApi.Interfaces;
using BlogApi.Models;
using System.Linq;
using BlogApi.Security;
using Microsoft.Extensions.Configuration;
using BlogApi.DTOs;
using System;

namespace BlogApi.Data
{
    public class IniciaSesion : ISesionRep
    {
        private readonly BlogHCContext _context;
        private string secretKey {get; set;}
        private string audienceToken {get; set;}
        private string issuerToken {get; set;}
        public IniciaSesion(BlogHCContext context, IConfiguration configuration)
        {
            _context = context;

             //Obtenemos las llaves de cifrado, de audience y de issuer desde el AppSettings.Json
            secretKey = configuration["JWTSettings:JWT_Secret"];
            audienceToken = configuration["JWTSettings:JWT_AUDIENCE_TOKEN"];
            issuerToken = configuration["JWTSettings:JWT_ISSUER_TOKEN"];
        }
        public UsuarioLeeDto Autenticacion(string usuario, string contrasena)
        {
            string Nombre = "Invitado";
            string Rol = "Libre";
            Usuario usuarioTem = new Usuario();
            DateTime ExpiresDate = DateTime.Now;

            if(usuario != "Invitado" && contrasena != "Invitado")
            {
                //Validación de la existencia del usuario
                usuarioTem = _context.Usuarios.FirstOrDefault(x => x.IdUsuario == usuario);
                if(usuarioTem == null)
                    return null;

                //Validación de que la contraseña se correcta
                string pass = RSAValidator.Decryption(usuarioTem.ContraUsua);
                if(!contrasena.Equals(pass))
                    return null;

                //Saber que rol y nombre de usario es
                Nombre =  usuarioTem.IdUsuario;
                Rol = usuarioTem.TipoUsua == 0 ? "Master" : usuarioTem.TipoUsua == 1 ? "Super" : usuarioTem.TipoUsua == 2 ? "Editor" : "Comun";

                //Verificar que el estatus del usuario sea activo
                if(usuarioTem.EstatusUsua == 0 && (Rol != "Master" || Rol != "Super"))
                    return null;
            }

            return new UsuarioLeeDto
            {
                IdUsuario = Nombre, 
                NombresUsua = usuarioTem.NombresUsua,
                ApellidosUsua = usuarioTem.ApellidosUsua,
                EstatusUsua = usuarioTem.EstatusUsua,
                Token = TokenGenVal.CrearToken(Nombre, Rol, secretKey, audienceToken, issuerToken, DateTime.Now.AddDays(7), out ExpiresDate),
                ExpiresDate = ExpiresDate,
                Rol = Rol,
                ProfilePicture = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Profiles/" + usuarioTem.ProfilePicture
            };
        }

        public UsuarioLeeDto RefreshToken(string usuario, string Token)
        {
            var usuarioTem = new Usuario();
            string Rol = "Invitado";
            DateTime ExpiresDate = DateTime.Now;

            if(usuario != "Invitado")
            {
                usuarioTem = _context.Usuarios.FirstOrDefault(x => x.IdUsuario == usuario);
                if(usuarioTem == null)
                    return null; 

                Rol = usuarioTem.TipoUsua == 0 ? "Master" : usuarioTem.TipoUsua == 1 ? "Super" : usuarioTem.TipoUsua == 2 ? "Editor" : "Comun";
            }

            if(!TokenGenVal.ValidacionTokenManual(Token, secretKey, audienceToken, issuerToken))
            {
                return null;
            }

            return new UsuarioLeeDto
            {
                IdUsuario = usuario, 
                NombresUsua = usuarioTem.NombresUsua,
                ApellidosUsua = usuarioTem.ApellidosUsua,
                EstatusUsua = usuarioTem.EstatusUsua,
                Token = TokenGenVal.CrearToken(usuario, Rol, secretKey, audienceToken, issuerToken, DateTime.Now.AddDays(7), out ExpiresDate),
                ExpiresDate = ExpiresDate,
                Rol = Rol
            };
        }
    }
}