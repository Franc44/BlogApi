using System;

namespace BlogApi.DTOs
{
    public class PublicacionLeeDto
    {
        public int IdPublicacion { get; set; }
        public string TituloPubli { get; set; }
        public string TextoPubli { get; set; }
        public string ImagenNombrePubli { get; set; }
        public DateTime FechaPubli { get; set; }
        public string UsuarioPubli { get; set; }
        public string NombreCompleto { get; set; }
    }
}