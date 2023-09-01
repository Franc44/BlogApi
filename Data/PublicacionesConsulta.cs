using BlogApi.Security;
using System.Collections.Generic;
using BlogApi.Interfaces;
using BlogApi.Models;
using System.Linq;
using System;
using AutoMapper;
using BlogApi.DTOs;

namespace BlogApi.Data
{
    public class PublicacionesConsulta : IPublicacionesRepo
    {
        private readonly BlogHCContext _context;
        private readonly IAccionUsuario _accion;
        private readonly IMapper _mapper;
        public PublicacionesConsulta(BlogHCContext context, IAccionUsuario accion, IMapper mapper)
        {
            _context = context;
            _accion = accion;
            _mapper = mapper;
        }
        public void ActualizaPublicacion(Publicacione publicacion)
        {
            
        }

        public void CrearPublicacion(Publicacione publicacion)
        {
            if(publicacion == null)
                throw new ArgumentNullException(nameof(publicacion));

            _context.Publicaciones.Add(publicacion);    
        }

        public void EliminaPublicacion(Publicacione publicacion)
        {
            if(publicacion == null)
            {
                throw new ArgumentNullException(nameof(publicacion));
            }

            _context.Publicaciones.Remove(publicacion);
        }

        public IEnumerable<PublicacionLeeDto> GetPublicaciones()
        {
            var publicacionesTem = _context.Publicaciones.OrderByDescending(x => x.FechaPubli).ToList();
            
            var Publicaciones = _mapper.Map<List<PublicacionLeeDto>>(publicacionesTem);

            for(int i = 0; i < publicacionesTem.Count; i++)
            {
               //Se trae la información del usuario que hizo la publicación
                var usuario = _context.Usuarios.Where(x => x.IdUsuario == publicacionesTem[i].UsuarioPubli).FirstOrDefault();

                var nombrecompletoDec = RSAValidator.Decryption(usuario.NombresUsua) + " " + RSAValidator.Decryption(usuario.ApellidosUsua);
                Publicaciones[i].NombreCompleto = nombrecompletoDec;
                Publicaciones[i].ImagenNombrePubli = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Images/" + publicacionesTem[i].ImagenNombrePubli;
            }
            
            return Publicaciones;
        }

        public Publicacione GetPublicacionPorId(int Id)
        {
            return _context.Publicaciones.FirstOrDefault(x => x.IdPublicacion == Id);
        }

        public IEnumerable<PublicacionLeeDto> GetPublicacionPorTipo(int tipo)
        {
            var publicacionesTem = _context.Publicaciones.Where(x => x.TipoPubli == tipo).OrderByDescending(x => x.FechaPubli).ToList();

            var Publicaciones = _mapper.Map<List<PublicacionLeeDto>>(publicacionesTem);

            for(int i = 0; i < publicacionesTem.Count; i++)
            {
                //Se trae la información del usuario que hizo la publicación
                var usuario = _context.Usuarios.Where(x => x.IdUsuario == publicacionesTem[i].UsuarioPubli).FirstOrDefault();

                var nombrecompletoDec = RSAValidator.Decryption(usuario.NombresUsua) + " " + RSAValidator.Decryption(usuario.ApellidosUsua);
                Publicaciones[i].NombreCompleto = nombrecompletoDec;
                Publicaciones[i].ImagenNombrePubli = "https://www.maxal-cloud.com.mx/blogapi/StaticFiles/Images/" + publicacionesTem[i].ImagenNombrePubli;
            }
            
            return Publicaciones;
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