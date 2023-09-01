using System;

namespace BlogApi.DTOs
{
    public class FPLikesDto
    {
        public int IdFplikes { get; set; }
        public int IdPreguntaFpl { get; set; }
        public string IdUsuarioFlp { get; set; }
        public DateTime FechaFlp { get; set; }
    }
}