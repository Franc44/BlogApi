using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BlogApi.DTOs
{
    public class DocumentoDto
    {
        public DateTime FechaDoc { get; set; }
        public string UsuarioDoc { get; set; }
        public int? EmpresaDoc { get; set; }
        public string TipoDoc { get; set; }
        public string TituloDoc { get; set; }
        public string DescripcionDoc { get; set; }
        public string FileDoc { get; set; }
        public IFormFile File { get; set;}
    }
}