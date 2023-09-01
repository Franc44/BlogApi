using System.Collections.Generic;
using BlogApi.Models;
using System.Linq;
using System;
using BlogApi.Interfaces;
using AutoMapper;
using BlogApi.DTOs;
using BlogApi.Security;
using BlogApi.Servicios;
using Microsoft.Extensions.Configuration;

namespace BlogApi.Data
{
    public class ObtencionUsuarios : IUsuariosRep
    {
        private readonly BlogHCContext _context;
        private readonly IMapper _mapper;
        private readonly IAccionUsuario _accion;
        private readonly IEmailService _emailService;
        private string secretKey {get; set;}
        private string audienceToken {get; set;}
        private string issuerToken {get; set;}
        public ObtencionUsuarios(BlogHCContext context, IMapper mapper, IAccionUsuario accion, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _accion = accion;
            _emailService = emailService;

             //Obtenemos las llaves de cifrado, de audience y de issuer desde el AppSettings.Json
            secretKey = configuration["JWTSettings:JWT_Secret"];
            audienceToken = configuration["JWTSettings:JWT_AUDIENCE_TOKEN"];
            issuerToken = configuration["JWTSettings:JWT_ISSUER_TOKEN"];
        }

        public void ActualizaUsuario(Usuario User)
        {
            //No necesitas nada
        }

        public bool CrearUsuario(Usuario User)
        {
            var UsuariosTem = _context.Usuarios.ToList();

            string matriculaUserDec = RSAValidator.Decryption(User.MatriculaUsua);
            string rfcUserDec = RSAValidator.Decryption(User.RfcUsua);
            string correoUserDec = RSAValidator.Decryption(User.EmailUsua);

            //Que usuario eres
            if(User.TipoUsua == 3)
            {
                //Validación de Matricula y RFC en la base de datos de HC
                bool valido = _context.RfcMatriculas.Where(x => x.Matricula == matriculaUserDec && x.Rfc == rfcUserDec).Count() > 0;
                if(!valido)
                    return false;
            }
            
            //Verifica que no exista un usuario identico en la base de datos
            foreach(var item in UsuariosTem)
            {
                if(User.IdUsuario == item.IdUsuario)
                    return false;

                if(User.TipoUsua == 3)
                {
                    string matriculaDec = RSAValidator.Decryption(item.MatriculaUsua);
                    string correoDec = RSAValidator.Decryption(item.EmailUsua);

                    if(matriculaDec == matriculaUserDec) return false;
                    if(correoDec == correoUserDec) return false;
                }
            }

            _context.Usuarios.Add(User);
            return true;
        }

        public void EliminaUsuario(Usuario User)
        {
            if(User == null)
            {
                throw new ArgumentNullException(nameof(User));
            }

            var documetosUsuarios = _context.Documentos.Where(x => x.UsuarioDoc == User.IdUsuario);
            _context.Documentos.RemoveRange(documetosUsuarios);
            var foroPreguntaUsuario = _context.ForoPreguntas.Where(x => x.UsuarioPregunta == User.IdUsuario);
            _context.ForoPreguntas.RemoveRange(foroPreguntaUsuario);
            //....

            _context.Usuarios.Remove(User);
        }

        public Usuario GetUsuarioPorId(string Id)
        {
            return _context.Usuarios.FirstOrDefault(x => x.IdUsuario == Id);
        }

        public IEnumerable<Usuario> GetUsuarios()
        {
            return _context.Usuarios.Where(x => x.TipoUsua != 0).ToList().OrderByDescending(x => x.FechaAltaUsua);
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

        public bool RecuperarContra(string id, out string Id_Usuario)
        {
            var listausuarios = _context.Usuarios.ToList();
            string correoDec = "", usuario = "";
            for(int i = 0; i < listausuarios.Count; i++)
            {
                correoDec = RSAValidator.Decryption(listausuarios[i].EmailUsua);
                string matriculaDec = RSAValidator.Decryption(listausuarios[i].MatriculaUsua);
                string rfcDec = RSAValidator.Decryption(listausuarios[i].RfcUsua);
                usuario = listausuarios[i].IdUsuario;

                if(matriculaDec != id && correoDec != id && listausuarios[i].IdUsuario != id && rfcDec != id)
                {
                    if(i == listausuarios.Count - 1)
                    {
                        Id_Usuario = "";
                        return false;                        
                    }
                }
                else
                    break;
            }  
            string claveRec = RandomKeys.Generate(6,0);

            var recuperaModel = new Recuperacion 
            {
                UsuarioRec = usuario,
                ClaveRec = claveRec,
                FechaRec = DateTime.Now,
                ExpiracionRec = DateTime.Now.AddHours(1)
            };
            
            _context.Recuperacions.Add(recuperaModel);
            _context.SaveChanges();

            EnviaMensajeRecuperacion(correoDec,claveRec, DateTime.Now.ToString());
            Id_Usuario = usuario;
            return true;
        }

        public string[] ValidacionClaveRecupera(string clave,string usuario)
        {
            bool existe = _context.Recuperacions.OrderByDescending(x => x.FechaRec)
                                                .Where(x => x.UsuarioRec == usuario && x.ClaveRec == clave && x.ExpiracionRec > DateTime.Now).Count() > 0;
            DateTime expires;

            if(existe)
            {
                decimal usuarioRol = _context.Usuarios.Where(x => x.IdUsuario == usuario).FirstOrDefault().TipoUsua;
                string Rol = usuarioRol == 0 ? "Master" : usuarioRol == 1 ? "Super" : usuarioRol == 2 ? "Editor" : "Comun";
                string[] tokenUsuario = {usuario, TokenGenVal.CrearToken(usuario, Rol, secretKey, audienceToken, issuerToken, DateTime.Now.AddHours(1), out expires)};
                return tokenUsuario;
            }
            
            return null;
        }

        private void EnviaMensajeRecuperacion(string correo, string clave, string fecha)
        {
            string message = $@"<p>Clave de recuperación de contraseña con fecha de expiración: {fecha}</p>
                             <p><a>{clave}</a></p>";

            _emailService.Send(
                to: correo,
                subject: "Recuperación de Contraseña",
                html: $@"<h4>Recuperación de contraseña</h4>
                         {message}"
            );
        }
    }
}