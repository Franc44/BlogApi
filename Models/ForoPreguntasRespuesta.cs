using System;
using System.Collections.Generic;

#nullable disable

namespace BlogApi.Models
{
    public partial class ForoPreguntasRespuesta
    {
        public ForoPreguntasRespuesta()
        {
            ForoPreguntasRespuestasLikes = new HashSet<ForoPreguntasRespuestasLike>();
        }

        public int IdRespuesta { get; set; }
        public string TextoRespuesta { get; set; }
        public decimal EstatusRespuesta { get; set; }
        public short LikesRespuesta { get; set; }
        public decimal CorrectaRespuesta { get; set; }
        public string UsuarioRespuesta { get; set; }
        public int PreguntaRespuesta { get; set; }
        public DateTime FechaRespuesta { get; set; }

        public virtual ForoPregunta PreguntaRespuestaNavigation { get; set; }
        public virtual Usuario UsuarioRespuestaNavigation { get; set; }
        public virtual ICollection<ForoPreguntasRespuestasLike> ForoPreguntasRespuestasLikes { get; set; }
    }
}
