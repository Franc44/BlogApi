using System;

namespace BlogApi.DTOs
{
    public class FPRLikesDto
    {
        public int IdFprlikes { get; set; }
        public int IdRespuestaFprl { get; set; }
        public string IdUsuarioFlrp { get; set; }
        public DateTime FechaFlrp { get; set; }
    }
}