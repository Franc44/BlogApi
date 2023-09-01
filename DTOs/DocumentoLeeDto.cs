using System;

namespace BlogApi.DTOs
{
    public class DocumentoLeeDto
    {
        public int IdDocumento { get; set; }
        public DateTime FechaDoc { get; set; }
        public string UsuarioDoc { get; set; }
        public string TipoDoc { get; set; }
        public string TituloDoc { get; set; }
        public string DescripcionDoc { get; set; }
        public string FileDoc { get; set; }
    }
}