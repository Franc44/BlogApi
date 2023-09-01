using System.Collections.Generic;
using BlogApi.Interfaces;
using BlogApi.Models;
using System.Linq;
using System;

namespace BlogApi.Data
{
    public class DocumentoConsultas : IDocumentosRepo
    {
        private readonly BlogHCContext _context;
        private readonly IAccionUsuario _accion;
        public DocumentoConsultas(BlogHCContext context, IAccionUsuario accion)
        {
            _context = context;
            _accion = accion;
        }
        public void ActualizaDocumento(Documento documento)
        {

        }

        public void CrearDocumento(Documento documento)
        {
            if(documento == null)
                throw new ArgumentNullException(nameof(documento));

            _context.Documentos.Add(documento); 
        }

        public void EliminaDocumento(Documento documento)
        {
            if(documento == null)
            {
                throw new ArgumentNullException(nameof(documento));
            }

            _context.Documentos.Remove(documento);
        }

        public IEnumerable<Documento> GetDocumento()
        {
            return _context.Documentos.OrderByDescending(x => x.FechaDoc);
        }

        public Documento GetDocumentoPorId(int Id)
        {
            return _context.Documentos.FirstOrDefault(x => x.IdDocumento == Id);
        }

        public IEnumerable<Documento> GetDocumentoPorTipo(string tipo)
        {
            return _context.Documentos.OrderByDescending(x => x.FechaDoc).Where(x => x.TipoDoc == tipo);
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