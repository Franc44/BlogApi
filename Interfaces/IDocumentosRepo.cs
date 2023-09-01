using System.Collections.Generic;
using BlogApi.Models;

namespace BlogApi.Interfaces
{
    public interface IDocumentosRepo
    {
        IEnumerable<Documento> GetDocumento();
        Documento GetDocumentoPorId(int Id);
        IEnumerable<Documento> GetDocumentoPorTipo(string tipo);
        bool GuardarCambio(string usuario);   
        void CrearDocumento(Documento documento);
        void ActualizaDocumento(Documento documento);
        void EliminaDocumento(Documento documento);
    }
}