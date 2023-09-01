using System;

namespace BlogApi.DTOs
{
    public class ForoPreguntasRespuestaDTO
    {
        public int IdRespuesta { get; set; }
        public string TextoRespuesta { get; set; }
        public decimal EstatusRespuesta { get; set; }
        public short LikesRespuesta { get; set; }
        public decimal CorrectaRespuesta { get; set; }
        public string UsuarioRespuesta { get; set; }
        public int PreguntaRespuesta { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public bool LikeDelUsuarioLog { get; set; }
        public string ProfilePicture { get; set; }
    }
}