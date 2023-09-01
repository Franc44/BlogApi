using BlogApi.Interfaces;
using BlogApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlogApi.Data
{
    public class AccionUsuarioConsulta : IAccionUsuario
    {
        private readonly BlogHCContext _context;
        public AccionUsuarioConsulta(BlogHCContext context)
        {
            _context = context;
        }

        public List<AccionesUsuario> GetAcciones()
        {
            return _context.AccionesUsuarios.ToList();
        }

        public void Usuario_Modifico_BD(string usuario)
        {
            var ultimoRegistroTrigger = _context.AccionesUsuarios.OrderBy(x => x.IdAccion).LastOrDefault();

            ultimoRegistroTrigger.UsuarioAccion = usuario;
            _context.SaveChanges();
        }
    }
}