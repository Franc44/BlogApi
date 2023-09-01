using System;
using System.Collections.Generic;

namespace BlogApi.DTOs
{
    public class ForoPreguntaLeeDto
    {
        public int IdPregunta { get; set; }
        public string TituloPregunta { get; set; }
        public string DescripcionPregunta { get; set; }
        public string[] Tags { get; set; }
        public short LikesPregunta { get; set; }
        public DateTime FechaPregunta { get; set; }
        public decimal EstatusPregunta { get; set; }
        public string UsuarioPregunta { get; set; }
        public int ContadorRespuestas { get; set; }        
        public bool LikeDelUsuarioLog { get; set; }
        public string ProfilePictureuUsuario{ get; set;}
        public List<ForoPreguntasRespuestaDTO> ForoPreguntasRespuesta { get; set; }
    }
}