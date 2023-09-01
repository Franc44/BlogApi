using System;
using Microsoft.AspNetCore.Http;

namespace BlogApi.DTOs
{
    public class PublicacionDTO
    {
        public string TituloPubli { get; set; }
        public string TextoPubli { get; set; }
        public string ImagenNombrePubli { get; set; }
        public DateTime FechaPubli { get; set; }
        public string UsuarioPubli { get; set; }
        public int? EmpresaPubli { get; set; }
        public decimal EstatusPubli { get; set; }
        public decimal TipoPubli { get; set; }
        public IFormFile File { get; set; }
    }
}